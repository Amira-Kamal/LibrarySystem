using LibrarySystem.Data;
using LibrarySystem.Models;
using LibrarySystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly LibraryDbContext _context;

        public CategoryRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories
                .ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories
                .FindAsync(id);
        }

        public async Task<Category?> GetDetailsAsync(int id)
        {
            return await _context.Categories
                .Include(c => c.Books)
                .FirstOrDefaultAsync(c => c.CategoryId == id);
        }

        public async Task AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories
                .FindAsync(id);

            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Categories
                .AnyAsync(c => c.CategoryId == id);
        }
    }
}
