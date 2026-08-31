using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Models
{
    public class Borrowing
    {
        public int BorrowingId { get; set; }

        public int BookId { get; set; }

        public string UserId { get; set; } = string.Empty;

        [Required]
        public DateTime BorrowDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        // Navigation Properties
        public Book Book { get; set; } = null!;

        public ApplicationUser User { get; set; } = null!;
    }
}
