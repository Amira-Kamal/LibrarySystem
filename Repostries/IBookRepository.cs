using LibrarySystem.Models;

namespace LibrarySystem.Repostries
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAllAsync();
        Task<List<Book>> GetAvailableAsync();
        Task<Book?> GetByIdAsync(int id);
        Task<Book?> FindAsync(int id);
        Task AddAsync(Book book);
        Task UpdateAsync(Book book);
        Task DeleteAsync(int id);
    }
}
