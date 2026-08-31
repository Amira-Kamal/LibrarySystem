using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "Full name must be between 2 and 100 characters")]
        public string FullName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Borrowing> Borrowings { get; set; }
              = new List<Borrowing>();
     }
}
