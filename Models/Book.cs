using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrarySystem.Models;

public class Book
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
    [Display(Name = "Publish Year")]
    public int PublishYear { get; set; }

    [Required]
    [Display(Name = "Available Copies")]
    public int AvailableCopies { get; set; }

    [Required]
    [Display(Name = "Total Copies")]
    public int TotalCopies { get; set; }

    [StringLength(500)]
    public string? Image { get; set; }

    [StringLength(500)]
    [Display(Name = "Book File")]
    public string? FilePath { get; set; }

    [Required]
    [ForeignKey(nameof(Category))]
    [Display(Name = "Category")]
    public int CategoryId { get; set; }

    public Category Category { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(Author))]
    [Display(Name = "Author")]
    public int AuthorId { get; set; }

    public Author Author { get; set; } = null!;

    public ICollection<Borrowing> Borrowings { get; set; } = new List<Borrowing>();
}