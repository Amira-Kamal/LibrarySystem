using LibrarySystem.Data;
using LibrarySystem.Models;
using LibrarySystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Controllers;

[Authorize]
public class BorrowingController : Controller
{
    private readonly LibraryDbContext _context;
    private readonly IBorrowingRepository _borrowingRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public BorrowingController(
        LibraryDbContext context,
        IBorrowingRepository borrowingRepository,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _borrowingRepository = borrowingRepository;
        _userManager = userManager;
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Index()
    {
        var borrowings = await _borrowingRepository.GetAllAsync();

        return View(borrowings);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var borrowing = await _borrowingRepository.GetByIdAsync(id.Value);

        if (borrowing == null)
            return NotFound();

        var currentUserId = _userManager.GetUserId(User);

        if (!User.IsInRole("Admin") && borrowing.UserId != currentUserId)
            return Forbid();

        return View(borrowing);
    }

    public IActionResult Create(int? bookId)
    {
        ViewData["BookId"] = new SelectList(
            _context.Books
                .Where(b => b.AvailableCopies > 0),
            "BookId",
            "Title",
            bookId
        );

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Borrowing borrowing)
    {
        ModelState.Remove("Book");
        ModelState.Remove("User");
        ModelState.Remove("UserId");

        var book = await _context.Books
            .FirstOrDefaultAsync(b => b.BookId == borrowing.BookId);

        if (book == null)
        {
            ModelState.AddModelError("", "Please select a valid book.");
        }
        else if (book.AvailableCopies <= 0)
        {
            ModelState.AddModelError("", "This book is not available.");
        }

        if (!ModelState.IsValid)
        {
            ViewData["BookId"] = new SelectList(
                _context.Books
                    .Where(b => b.AvailableCopies > 0),
                "BookId",
                "Title",
                borrowing.BookId
            );

            return View(borrowing);
        }

        var userId = _userManager.GetUserId(User);

        if (userId == null)
            return Challenge();

        borrowing.UserId = userId;
        borrowing.BorrowDate = DateTime.Now;
        borrowing.ReturnDate = null;

        book.AvailableCopies--;

        await _borrowingRepository.AddAsync(borrowing);
        await _borrowingRepository.SaveAsync();

        return RedirectToAction(nameof(MyBorrowings));
    }

    public async Task<IActionResult> MyBorrowings()
    {
        var userId = _userManager.GetUserId(User);

        if (userId == null)
            return Challenge();

        var borrowings =
            await _borrowingRepository.GetByUserIdAsync(userId);

        return View(borrowings);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Return(int id)
    {
        var borrowing =
            await _borrowingRepository.GetByIdAsync(id);

        if (borrowing == null)
            return NotFound();

        var currentUserId = _userManager.GetUserId(User);

        if (!User.IsInRole("Admin") &&
            borrowing.UserId != currentUserId)
            return Forbid();

        if (borrowing.ReturnDate != null)
        {
            TempData["Error"] = "Book has already been returned.";
            return RedirectToAction(nameof(MyBorrowings));
        }

        borrowing.ReturnDate = DateTime.Now;

        if (borrowing.Book != null)
        {
            borrowing.Book.AvailableCopies++;
        }

        await _borrowingRepository.SaveAsync();

        return RedirectToAction(nameof(MyBorrowings));
    }
}