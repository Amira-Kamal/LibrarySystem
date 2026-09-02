using LibrarySystem.Models;

namespace LibrarySystem.ViewModels
{
    public class CategoryDetailsViewModel
    {
        public int CategoryId { get; set; }

        public string Name { get; set; }

        public List<Book> Books { get; set; } = new List<Book>();
    }
}