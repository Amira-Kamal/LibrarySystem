using LibrarySystem.Data;
using LibrarySystem.Models;
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
    private readonly UserManager<ApplicationUser> _userManager;

    public BorrowingController(
        LibraryDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Index()
    {
        var borrowings = _context.Borrowings
            .Include(b => b.Book)
            .Include(b => b.User);

        return View(await borrowings.ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var borrowing = await _context.Borrowings
            .Include(b => b.Book)
            .Include(b => b.User)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (borrowing == null)
            return NotFound();

        var currentUserId = _userManager.GetUserId(User);

        if (!User.IsInRole("Admin") && borrowing.UserId != currentUserId)
            return Forbid();

        return View(borrowing);
    }

    public IActionResult Create()
    {
        ViewData["BookId"] = new SelectList(
            _context.Books.Where(b => b.AvailableCopies > 0),
            "BookId",
            "Title"
        );

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Borrowing borrowing)
    {
        ModelState.Remove("Book");
        ModelState.Remove("User");

        var book = await _context.Books.FindAsync(borrowing.BookId);

        if (book == null || book.AvailableCopies <= 0)
        {
            ModelState.AddModelError("", "Book is not available.");
        }

        if (ModelState.IsValid && book != null && book.AvailableCopies > 0)
        {
            borrowing.UserId = _userManager.GetUserId(User)!;
            borrowing.BorrowDate = DateTime.Now;
            borrowing.ReturnDate = null;

            book.AvailableCopies--;

            _context.Borrowings.Add(borrowing);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(MyBorrowings));
        }

        ViewData["BookId"] = new SelectList(
            _context.Books.Where(b => b.AvailableCopies > 0),
            "BookId",
            "Title",
            borrowing.BookId
        );

        return View(borrowing);
    }

    public async Task<IActionResult> MyBorrowings()
    {
        var userId = _userManager.GetUserId(User);

        var borrowings = _context.Borrowings
            .Include(b => b.Book)
            .Where(b => b.UserId == userId && b.ReturnDate == null);

        return View(await borrowings.ToListAsync());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Return(int id)
    {
        var borrowing = await _context.Borrowings
            .Include(b => b.Book)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (borrowing == null)
            return NotFound();

        var currentUserId = _userManager.GetUserId(User);

        if (!User.IsInRole("Admin") && borrowing.UserId != currentUserId)
            return Forbid();

        if (borrowing.ReturnDate != null)
        {
            TempData["Error"] = "Book already returned.";
            return RedirectToAction(nameof(MyBorrowings));
        }

        borrowing.ReturnDate = DateTime.Now;
        borrowing.Book.AvailableCopies++;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(MyBorrowings));
    }
}