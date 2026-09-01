using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Models;

public class Author
{
    [Key]
    public int AuthorId { get; set; }

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string LastName { get; set; } = string.Empty;

    public string? Bio { get; set; }

    [StringLength(50)]
    public string? Nationality { get; set; }

    public ICollection<Book> Books { get; set; } = new List<Book>();
}