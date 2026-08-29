using LibrarySystem.Data;
using LibrarySystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Repositories;

public class BorrowingRepository : IBorrowingRepository
{
    private readonly LibraryDbContext _context;

    public BorrowingRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Borrowing>> GetAllAsync()
    {
        return await _context.Borrowings
            .Include(b => b.Book)
            .Include(b => b.User)
            .ToListAsync();
    }

    public async Task<Borrowing?> GetByIdAsync(int id)
    {
        return await _context.Borrowings
            .Include(b => b.Book)
            .Include(b => b.User)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<IEnumerable<Borrowing>> GetByUserIdAsync(string userId)
    {
        return await _context.Borrowings
            .Include(b => b.Book)
            .Where(b => b.UserId == userId && b.ReturnDate == null)
            .OrderByDescending(b => b.BorrowDate)
            .ToListAsync();
    }

    public async Task AddAsync(Borrowing borrowing)
    {
        await _context.Borrowings.AddAsync(borrowing);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}