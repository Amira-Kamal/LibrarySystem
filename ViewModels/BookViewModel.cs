using LibrarySystem.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.ViewModels
{
    public class BookViewModel
    {
        [Key]
        public int BookId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        [StringLength(20)]
        public string ISBN { get; set; } = string.Empty;

        [Required]
        public int PublishYear { get; set; }

        [Required]
        public int AvailableCopies { get; set; }

        [Required]
        public int TotalCopies { get; set; }

        [StringLength(500)]
        public string? Image { get; set; }
        [Required]
        [StringLength(500)]
       // [Display(Name = "Book File")]
        public string? FilePath { get; set; }
        public string CategoryName { get; set; }
        [Required]
        public string AuthorFirstName { get; set; }
        [Required]
        public string AuthorLastName { get; set; }









    }
}
