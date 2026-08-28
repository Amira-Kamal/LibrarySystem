
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibrarySystem.Models;
using LibrarySystem.Data;

public class BorrowingsController : Controller
{
    private readonly LibraryDbContext _context;

    public BorrowingsController(LibraryDbContext context)
    {
        _context = context;
    }

    // GET: BORROWINGS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Borrowings.ToListAsync());
    }

    // GET: BORROWINGS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var borrowing = await _context.Borrowings
            .FirstOrDefaultAsync(m => m.Id == id);
        if (borrowing == null)
        {
            return NotFound();
        }

        return View(borrowing);
    }

    // GET: BORROWINGS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: BORROWINGS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,BookId,Book,UserId,User,BorrowDate,ReturnDate")] Borrowing borrowing)
    {
        if (ModelState.IsValid)
        {
            _context.Add(borrowing);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(borrowing);
    }

    // GET: BORROWINGS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var borrowing = await _context.Borrowings.FindAsync(id);
        if (borrowing == null)
        {
            return NotFound();
        }
        return View(borrowing);
    }

    // POST: BORROWINGS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,BookId,Book,UserId,User,BorrowDate,ReturnDate")] Borrowing borrowing)
    {
        if (id != borrowing.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(borrowing);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BorrowingExists(borrowing.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(borrowing);
    }

    // GET: BORROWINGS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var borrowing = await _context.Borrowings
            .FirstOrDefaultAsync(m => m.Id == id);
        if (borrowing == null)
        {
            return NotFound();
        }

        return View(borrowing);
    }

    // POST: BORROWINGS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var borrowing = await _context.Borrowings.FindAsync(id);
        if (borrowing != null)
        {
            _context.Borrowings.Remove(borrowing);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool BorrowingExists(int? id)
    {
        return _context.Borrowings.Any(e => e.Id == id);
    }
}
