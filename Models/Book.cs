using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Models
{
    public class Book
    {
        public int BookId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, MinimumLength = 2,
            ErrorMessage = "Title must be between 2 and 200 characters")]
        public string Title { get; set; } = string.Empty;

        [StringLength(2000,
            ErrorMessage = "Description cannot exceed 2000 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "ISBN is required")]
        [StringLength(20, MinimumLength = 10,
            ErrorMessage = "ISBN must be between 10 and 20 characters")]
        public string ISBN { get; set; } = string.Empty;

        [Range(1000, 2100,
            ErrorMessage = "Please enter a valid publish year")]
        public int PublishYear { get; set; }

        [StringLength(500)]
        public string? Image { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Total copies must be at least 1")]
        public int TotalCopies { get; set; }

        [Range(0, int.MaxValue,
            ErrorMessage = "Available copies cannot be negative")]
        public int AvailableCopies { get; set; }

        // Foreign Keys
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }

        // Navigation Properties
        public Author Author { get; set; } = null!;
        public Category Category { get; set; } = null!;

        public ICollection<Borrowing> Borrowings { get; set; }
            = new List<Borrowing>();
    }
}
