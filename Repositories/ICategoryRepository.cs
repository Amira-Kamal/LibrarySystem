using LibrarySystem.Models;

namespace LibrarySystem.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();

        Task<Category?> GetByIdAsync(int id);

        Task<Category?> GetDetailsAsync(int id);

        Task AddAsync(Category category);

        Task UpdateAsync(Category category);

        Task DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);
    }
}
