using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrarySystem.Models;

public class Borrowing
{
    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey(nameof(Book))]
    [Display(Name = "Book")]
    public int BookId { get; set; }

    public Book Book { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(User))]
    [Display(Name = "User")]
    public string UserId { get; set; } = string.Empty;

    public ApplicationUser User { get; set; } = null!;

    [Required]
    [Display(Name = "Borrow Date")]
    public DateTime BorrowDate { get; set; } = DateTime.Now;

    [Display(Name = "Return Date")]
    public DateTime? ReturnDate { get; set; }
}