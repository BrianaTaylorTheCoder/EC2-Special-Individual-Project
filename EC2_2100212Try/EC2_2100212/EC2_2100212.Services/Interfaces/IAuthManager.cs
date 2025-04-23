using EC2_2100212.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace EC2_2100212.Services.Interfaces
{
    public interface IAuthManager
    {
        Task<StatusViewModel> LoginAsync(LoginViewModel model);
        Task LogoutAsync();
        Task<StatusViewModel> RegisterAsync(UserViewModel model);
        Task<IdentityResult> CreateUserAsync(UserViewModel user);
        Task<IdentityResult> UpdateUserAsync(UserViewModel user);
        Task<IdentityResult> DeleteUserAsync(UserViewModel user);
        Task<IdentityRole> FindRoleByIdAsync(string id);
        Task<UserViewModel> FindUserByIdAsync(string id);
        Task<bool> IsInRoleAsync(UserViewModel model, string role);
        Task<List<UserViewModel>> GetAllUsersAsync();
        List<RoleViewModel> GetAllRolesAsync();
        Task<bool> RoleExistsAsync(string name);
        Task<IdentityResult> AddUserToRoleAsync(UserViewModel model, string role);
        Task<IdentityResult> AddUserToRolesAsync(UserViewModel model, IList<string> roles);
        Task<IdentityResult> RemoveUserFromRoleAsync(UserViewModel model, string role);
        Task<IdentityResult> RemoveUserFromRolesAsync(UserViewModel model, IList<string> roles);
        Task<IdentityResult> UpdateRoleAsync(IdentityRole role);
        Task<IdentityResult> CreateRoleAsync(IdentityRole role);
        Task<IdentityResult> DeleteRoleAsync(IdentityRole role);
        Task<IList<string>> GetUserRolesAsync(UserViewModel model);

    }
}
