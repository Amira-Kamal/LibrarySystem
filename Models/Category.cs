using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Models
{
    public class Category
    {
            public int CategoryId { get; set; }

            [Required(ErrorMessage = "Category name is required")]
            [StringLength(100, MinimumLength = 2,
                ErrorMessage = "Category name must be between 2 and 100 characters")]
            public string Name { get; set; } = string.Empty;

            // Navigation Property
            public ICollection<Book> Books { get; set; } = new List<Book>();
        
    }
}

