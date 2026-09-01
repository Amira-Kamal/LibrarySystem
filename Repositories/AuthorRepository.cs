using LibrarySystem.Data;
using LibrarySystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly LibraryDbContext _context;

        public AuthorRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Author> GetAll()
        {
            return _context.Authors.AsNoTracking().ToList();
        }

        public Author? GetById(int id)
        {
            return _context.Authors.FirstOrDefault(a => a.AuthorId == id);
        }

        public void Add(Author author)
        {
            _context.Authors.Add(author);
        }

        public void Update(Author author)
        {
            _context.Authors.Update(author);
        }

        public void Delete(int id)
        {
            var author = GetById(id);
            if (author != null)
            {
                _context.Authors.Remove(author);
            }
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}