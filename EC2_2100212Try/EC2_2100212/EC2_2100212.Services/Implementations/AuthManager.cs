using AutoMapper;
using Microsoft.AspNetCore.Identity;
using EC2_2100212.Models;
using EC2_2100212.ViewModels;
using EC2_2100212.Services.Interfaces;
using EC2_2100212.Repositories.Interfaces;

namespace EC2_2100212.Services.Implementations
{
    public class AuthManager : IAuthManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;

        public AuthManager(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }


        public async Task<StatusViewModel> LoginAsync(LoginViewModel model)
        {
            var status = new StatusViewModel();
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user == null)
            {
                status.StatusCode = 0;
                status.Message = "Invalid Username";
                return status;
            }

            if (!await _userManager.CheckPasswordAsync(user, model.Password))
            {
                status.StatusCode = 0;
                status.Message = "Invalid Password";
                return status;
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (result.Succeeded)
            {
                status.StatusCode = 1;
                status.Message = "Logged In Successfully";
            }
            else if (result.IsLockedOut)
            {
                status.StatusCode = 0;
                status.Message = "User is Locked Out";
            }
            else
            {
                status.StatusCode = 0;
                status.Message = "Error With Log In";
            }
            return status;
        }
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
        public async Task<StatusViewModel> RegisterAsync(UserViewModel model)
        {
            var status = new StatusViewModel();
            var userExists = await _userManager.FindByNameAsync(model.UserName);
            if (userExists != null)
            {
                status.StatusCode = 0;
                status.Message = "Username Already Exists";
                return status;
            }
            ApplicationUser user = new ApplicationUser
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                Name = model.Name,
                Image = model.Image,
                Gender = model.Gender,
                Address = model.Address,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                status.StatusCode = 0;
                status.Message = "User Creation Failed";
                return status;
            }
            IdentityResult saveRoleResult = await _userManager.AddToRoleAsync(user, "Customer");

            status.StatusCode = 1;
            status.Message = "You Have Registered Successfully";
            return status;
        }


        public async Task<IdentityResult> CreateUserAsync(UserViewModel model)
        {
            var user = _mapper.Map<ApplicationUser>(model);
            return await _userManager.CreateAsync(user, model.Password);
        }
        public async Task<IdentityResult> UpdateUserAsync(UserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            user.Name = model.Name;
            user.Address = model.Address;
            user.Email = model.Email;
            user.UserName = model.UserName;
            user.Image = model.Image;
            user.Gender = model.Gender;

            return await _userManager.UpdateAsync(user);
        }
        public async Task<IdentityResult> DeleteUserAsync(UserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            return await _userManager.DeleteAsync(user);
        }

        public async Task<UserViewModel> FindUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return _mapper.Map<UserViewModel>(user);
        }
        public async Task<List<UserViewModel>> GetAllUsersAsync()
        {
            return _mapper.Map<List<UserViewModel>>(_userManager.Users.ToList());
        }

        public async Task<IdentityResult> AddUserToRoleAsync(UserViewModel user, string role)
        {
            return await _userManager.AddToRoleAsync(_mapper.Map<ApplicationUser>(user), role);
        }
        public async Task<IdentityResult> AddUserToRolesAsync(UserViewModel model, IList<string> roles)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            return await _userManager.AddToRolesAsync(user, roles);
        }
        public async Task<IdentityResult> RemoveUserFromRoleAsync(UserViewModel user, string role)
        {
            return await _userManager.RemoveFromRoleAsync(_mapper.Map<ApplicationUser>(user), role);
        }
        public async Task<IdentityResult> RemoveUserFromRolesAsync(UserViewModel model, IList<string> roles)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            return await _userManager.RemoveFromRolesAsync(user, roles);
        }
        public async Task<bool> IsInRoleAsync(UserViewModel model, string role)
        {
            return await _userManager.IsInRoleAsync(_mapper.Map<ApplicationUser>(model), role);
        }
        public async Task<IdentityResult> UpdateRoleAsync(IdentityRole role)
        {
            return await _roleManager.UpdateAsync(role);
        }
        public async Task<bool> RoleExistsAsync(string name)
        {
            return await _roleManager.RoleExistsAsync(name);
        }


        public async Task<IdentityResult> CreateRoleAsync(IdentityRole role)
        {
            return await _roleManager.CreateAsync(role);
        }
        public async Task<IdentityResult> DeleteRoleAsync(IdentityRole model)
        {
            var role = _roleManager.FindByIdAsync(model.Id);
            return await _roleManager.DeleteAsync(role.Result);
        }
        public List<RoleViewModel> GetAllRolesAsync()
        {
            var roles = _roleManager.Roles.ToList();
            return _mapper.Map<List<RoleViewModel>>(roles);
        }
        public async Task<IList<string>> GetUserRolesAsync(UserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            return await _userManager.GetRolesAsync(user);
        }
        public async Task<IdentityRole> FindRoleByIdAsync(string id)
        {
            return await _roleManager.FindByIdAsync(id);
        }

    }
}
