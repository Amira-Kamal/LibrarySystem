using LibrarySystem.Data;
using LibrarySystem.Models;
using LibrarySystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Controllers;

public class BookController : Controller
{
    private readonly LibraryDbContext _context;
    private readonly IWebHostEnvironment _env;

    public BookController(
        LibraryDbContext context,
        IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public async Task<IActionResult> Index()
    {
        var books = _context.Books
            .Include(b => b.Category)
            .Include(b => b.Author);

        return View(await books.ToListAsync());
    }

    public async Task<IActionResult> Available()
    {
        var books = _context.Books
            .Include(b => b.Category)
            .Include(b => b.Author)
            .Where(b => b.AvailableCopies > 0);

        return View("Index", await books.ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var book = await _context.Books
            .Include(b => b.Category)
            .Include(b => b.Author)
            .FirstOrDefaultAsync(m => m.BookId == id);

        if (book == null)
            return NotFound();

        var viewModel = new BookViewModel
        {
            BookId = book.BookId,
            Title = book.Title,
            Description = book.Description,
            ISBN = book.ISBN,
            PublishYear = book.PublishYear,
            AvailableCopies = book.AvailableCopies,
            TotalCopies = book.TotalCopies,
            Image = book.Image,
            FilePath = book.FilePath,
            CategoryName = book.Category.Name,
            AuthorFirstName = book.Author.FirstName,
            AuthorLastName =book.Author.LastName

        };     

        return View(viewModel);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        ViewData["CategoryId"] = new SelectList(
            _context.Categories,
            "CategoryId",
            "Name"
        );

        ViewData["AuthorId"] = new SelectList(
            _context.Authors,
            "AuthorId",
            "FirstName"
        );

        return View();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Book book)
    {
        ModelState.Remove("Category");
        ModelState.Remove("Author");
        ModelState.Remove("Borrowings");

        if (ModelState.IsValid)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        ViewData["CategoryId"] = new SelectList(
            _context.Categories,
            "CategoryId",
            "Name",
            book.CategoryId
        );

        ViewData["AuthorId"] = new SelectList(
            _context.Authors,
            "AuthorId",
            "FirstName",
            book.AuthorId
        );

        return View(book);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var book = await _context.Books.FindAsync(id);

        if (book == null)
            return NotFound();

        ViewData["CategoryId"] = new SelectList(
            _context.Categories,
            "CategoryId",
            "Name",
            book.CategoryId
        );

        ViewData["AuthorId"] = new SelectList(
            _context.Authors,
            "AuthorId",
            "FirstName",
            book.AuthorId
        );

        return View(book);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Book book)
    {
        if (id != book.BookId)
            return NotFound();

        ModelState.Remove("Category");
        ModelState.Remove("Author");
        ModelState.Remove("Borrowings");

        if (ModelState.IsValid)
        {
            _context.Update(book);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        ViewData["CategoryId"] = new SelectList(
            _context.Categories,
            "CategoryId",
            "Name",
            book.CategoryId
        );

        ViewData["AuthorId"] = new SelectList(
            _context.Authors,
            "AuthorId",
            "FirstName",
            book.AuthorId
        );

        return View(book);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var book = await _context.Books
            .Include(b => b.Category)
            .Include(b => b.Author)
            .FirstOrDefaultAsync(m => m.BookId == id);

        if (book == null)
            return NotFound();

        return View(book);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var book = await _context.Books.FindAsync(id);

        if (book != null)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    [AllowAnonymous]
    public async Task<IActionResult> Read(int id)
    {
        var book = await _context.Books.FindAsync(id);

        if (book == null || string.IsNullOrEmpty(book.FilePath))
            return NotFound("الكتاب غير متاح للقراءة حاليًا.");

        return View(book);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UploadFile(int id, IFormFile file)
    {
        var book = await _context.Books.FindAsync(id);

        if (book == null)
            return NotFound();

        if (file == null || file.Length == 0)
            return RedirectToAction(nameof(Details), new { id });

        if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
            return RedirectToAction(nameof(Details), new { id });

        var folderPath = Path.Combine(_env.WebRootPath, "BookFiles");

        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        var fileName = $"{id}_{Guid.NewGuid()}.pdf";
        var fullPath = Path.Combine(folderPath, fileName);

        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        book.FilePath = $"/BookFiles/{fileName}";

        _context.Update(book);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new { id });
    }
}