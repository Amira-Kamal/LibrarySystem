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

    // Admin: View all borrowings
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Index()
    {
        var borrowings = await _context.Borrowings
            .Include(b => b.Book)
            .Include(b => b.User)
            .ToListAsync();

        return View(borrowings);
    }

    // View borrowing details
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var borrowing = await _context.Borrowings
            .Include(b => b.Book)
            .Include(b => b.User)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (borrowing == null)
            return NotFound();

        var currentUserId = _userManager.GetUserId(User);

        if (!User.IsInRole("Admin") && borrowing.UserId != currentUserId)
            return Forbid();

        return View(borrowing);
    }

    // Display borrowing page
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

    // Create borrowing
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Borrowing borrowing)
    {
        // User and Book navigation properties are not entered from the form
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

        // Decrease available copies
        book.AvailableCopies--;

        _context.Borrowings.Add(borrowing);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(MyBorrowings));
    }

    // Current user's borrowed books
    public async Task<IActionResult> MyBorrowings()
    {
        var userId = _userManager.GetUserId(User);

        if (userId == null)
            return Challenge();

        var borrowings = await _context.Borrowings
            .Include(b => b.Book)
            .Where(b => b.UserId == userId && b.ReturnDate == null)
            .OrderByDescending(b => b.BorrowDate)
            .ToListAsync();

        return View(borrowings);
    }

    // Return borrowed book
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
            TempData["Error"] = "Book has already been returned.";
            return RedirectToAction(nameof(MyBorrowings));
        }

        borrowing.ReturnDate = DateTime.Now;

        // Increase available copies
        borrowing.Book.AvailableCopies++;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(MyBorrowings));
    }


}
