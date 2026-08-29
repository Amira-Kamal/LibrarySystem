using LibrarySystem.Data;
using LibrarySystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Controllers;

[Authorize(Roles = "Admin")]
public class CategoryController : Controller
{
    private readonly LibraryDbContext _context;

    public CategoryController(LibraryDbContext context)
    {
        _context = context;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        return View(await _context.Categories.ToListAsync());
    }

    
    [AllowAnonymous]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var category = await _context.Categories
            .Include(c => c.Books)
            .ThenInclude(b => b.Author)
            .FirstOrDefaultAsync(c => c.CategoryId == id);

        return category == null ? NotFound() : View(category);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category category)
    {
        if (ModelState.IsValid)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(category);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var category = await _context.Categories.FindAsync(id);

        return category == null ? NotFound() : View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Category category)
    {
        if (id != category.CategoryId)
            return NotFound();

        if (ModelState.IsValid)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(category);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var category = await _context.Categories
            .FirstOrDefaultAsync(m => m.CategoryId == id);

        return category == null ? NotFound() : View(category);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category != null)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}