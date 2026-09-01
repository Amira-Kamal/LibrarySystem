using LibrarySystem.Models;

namespace LibrarySystem.Repositories
{
    public interface IAuthorRepository
    {
        IEnumerable<Author> GetAll();
        Author? GetById(int id);
        void Add(Author author);
        void Update(Author author);
        void Delete(int id);
        bool Save();
    }
}