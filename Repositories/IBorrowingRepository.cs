using LibrarySystem.Models;

namespace LibrarySystem.Repositories;

public interface IBorrowingRepository
{
    Task<IEnumerable<Borrowing>> GetAllAsync();
    Task<Borrowing?> GetByIdAsync(int id);
    Task<IEnumerable<Borrowing>> GetByUserIdAsync(string userId);
    Task AddAsync(Borrowing borrowing);
    Task SaveAsync();
}