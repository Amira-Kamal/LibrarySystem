using LibrarySystem.Models;
using Microsoft.AspNetCore.Identity;

namespace LibrarySystem.Repositories
{
    public interface IAccountRepository
    {
        Task<ApplicationUser?> FindByEmailAsync(string email);

        Task<IdentityResult> CreateUserAsync(
            ApplicationUser user,
            string password);

        Task<IdentityResult> AddToRoleAsync(
            ApplicationUser user,
            string role);

        Task<bool> CheckPasswordAsync(
            ApplicationUser user,
            string password);
    }
}
