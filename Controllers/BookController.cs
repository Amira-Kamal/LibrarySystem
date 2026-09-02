using LibrarySystem.Data;
using LibrarySystem.Models;
using LibrarySystem.Repositories;
using LibrarySystem.Repostries;
using LibrarySystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Controllers;

public class BookController : Controller
{
    private readonly IBookRepository _bookRepository;
    private readonly LibraryDbContext _context;
    private readonly IWebHostEnvironment _env;

    public BookController(
        IBookRepository bookRepository,
        LibraryDbContext context,
        IWebHostEnvironment env)
    {
        _bookRepository = bookRepository;
        _context = context;
        _env = env;
    }


    public async Task<IActionResult> Index()
    {
        var books = await _bookRepository.GetAllAsync();

        return View(books);
    }


    public async Task<IActionResult> Available()
    {
        var books = await _bookRepository.GetAvailableAsync();

        return View("Index", books);
    }


    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var book = await _bookRepository.GetByIdAsync(id.Value);

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
            AuthorLastName = book.Author.LastName
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
            await _bookRepository.AddAsync(book);

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

        var book = await _bookRepository.FindAsync(id.Value);

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
            await _bookRepository.UpdateAsync(book);

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

        var book = await _bookRepository.GetByIdAsync(id.Value);

        if (book == null)
            return NotFound();

        return View(book);
    }



    [Authorize(Roles = "Admin")]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _bookRepository.DeleteAsync(id);

        return RedirectToAction(nameof(Index));
    }



    [Authorize]
    public async Task<IActionResult> Read(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var hasBorrowed = await _context.Borrowings
            .AnyAsync(b => b.BookId == id && b.UserId == userId);

        if (!hasBorrowed)
        {
            return View("~/Views/Account/AccessDenied.cshtml");
        }

        var book = await _bookRepository.FindAsync(id);

        if (book == null || string.IsNullOrEmpty(book.FilePath))
            return NotFound("Book not available for reading currently.");

        return View(book);
    }




    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UploadFile(int id, IFormFile file)
    {
        var book = await _bookRepository.FindAsync(id);

        if (book == null)
            return NotFound();

        if (file == null || file.Length == 0)
            return RedirectToAction(nameof(Details), new { id });


        

        if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
            return RedirectToAction(nameof(Details), new { id });


        

        var folderPath = Path.Combine(
            _env.WebRootPath,
            "BookFiles"
        );

        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);


        var fileName = $"{id}_{Guid.NewGuid()}.pdf";

        var fullPath = Path.Combine(
            folderPath,
            fileName
        );


        using (var stream = new FileStream(
            fullPath,
            FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }


        book.FilePath = $"/BookFiles/{fileName}";

        await _bookRepository.UpdateAsync(book);

        return RedirectToAction(nameof(Details), new { id });
    }
}