using LibrarySystem.Data;
using LibrarySystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Repostries
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext _context;

        public BookRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<List<Book>> GetAllAsync()
        {
            return await _context.Books
                .Include(b => b.Category)
                .Include(b => b.Author)
                .ToListAsync();
        }


        public async Task<List<Book>> GetAvailableAsync()
        {
            return await _context.Books
                .Include(b => b.Category)
                .Include(b => b.Author)
                .Where(b => b.AvailableCopies > 0)
                .ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            return await _context.Books
                .Include(b => b.Category)
                .Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.BookId == id);
        }

        public async Task<Book?> FindAsync(int id)
        {
            return await _context.Books
                .FindAsync(id);
        }

        public async Task AddAsync(Book book)
        {
            await _context.Books.AddAsync(book);

            await _context.SaveChangesAsync();
        }



        public async Task UpdateAsync(Book book)
        {
            _context.Books.Update(book);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book != null)
            {
                _context.Books.Remove(book);

                await _context.SaveChangesAsync();
            }
        }
    }
}
