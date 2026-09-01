using LibrarySystem.Data;
using LibrarySystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Controllers;

[Authorize(Roles = "Admin")]
public class AuthorController : Controller
{
    private readonly LibraryDbContext _context;

    public AuthorController(LibraryDbContext context)
    {
        _context = context;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        return View(await _context.Authors.ToListAsync());
    }

    [AllowAnonymous]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var author = await _context.Authors
            .FirstOrDefaultAsync(m => m.AuthorId == id);

        return author == null ? NotFound() : View(author);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Author author)
    {
        if (ModelState.IsValid)
        {
            _context.Add(author);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(author);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var author = await _context.Authors.FindAsync(id);
        return author == null ? NotFound() : View(author);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Author author)
    {
        if (id != author.AuthorId)
            return NotFound();

        if (ModelState.IsValid)
        {
            _context.Update(author);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(author);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var author = await _context.Authors
            .FirstOrDefaultAsync(m => m.AuthorId == id);

        return author == null ? NotFound() : View(author);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var author = await _context.Authors.FindAsync(id);

        if (author != null)
            _context.Authors.Remove(author);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}