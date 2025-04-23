
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BNS2025.ViewModels;
using BNS2025.Services.Interfaces;
using BNS2025.Web.Repositories;

namespace BNS2025.Web.Controllers
{
   
    [Authorize(Roles ="Administrator")]
    [Authorize(Roles = "Teller")]
    public class AdministrationController : Controller
    {
        private readonly IAuthManager _authManager;
        private readonly IFileService _fileService;
        //private readonly IAlertService _alertHelper;

        public AdministrationController(IAuthManager authManager, IFileService fileService)
        {
            _authManager = authManager;
            _fileService = fileService;
            
        }


        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleViewModel model)
        {

            if (model.Name != null)
            {
                IdentityRole identityRole = new ()
                {
                    Name = model.Name
                };

                IdentityResult res = await _authManager.CreateRoleAsync(identityRole);

                if (res.Succeeded)
                {
                    TempData["SuccessMessage"] = "Role created!";
                    return RedirectToAction("ListRoles", "Administration");
                }

                foreach (var error in res.Errors)
                {
                    TempData["ErrorMessage"] = ("Role not created!");
                    ModelState.AddModelError("", $"Error: {error.Description}");
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string Id)
        {
            var role = await _authManager.FindRoleByIdAsync(Id);

            if (role == null)
            {
                TempData["ErrorMessage"] = ("Role not found");
                ViewBag.ErrorMessage = $"Role with ID = {Id} cannot be found";
                return View("NotFound");
            }

            var model = new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name
            };
            var users = await _authManager.GetAllUsersAsync();
            model.Users = new List<UserViewModel>();
            foreach (var user in users)
            {
                if (await _authManager.IsInRoleAsync(user, role.Name))
                    model.Users.Add(user);
            }

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> EditRole(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {

                bool roleNameExists = await _authManager.RoleExistsAsync(model.Name);

                if (!roleNameExists)
                {
                    var role = await _authManager.FindRoleByIdAsync(model.Id);
                    if (role != null)
                    {
                        role.Name = model.Name;
                        var res = await _authManager.UpdateRoleAsync(role);

                        if (res.Succeeded)
                        {
                            TempData["SuccessMessage"] = "Role updated successfully!";
                            return RedirectToAction("ListRoles", "Administration");
                        }

                        TempData["ErrorMessage"] = ("Role creation failed");
                        foreach (var error in res.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                    ModelState.AddModelError("", "Role Id Does Not Exists in Database");
                }
                ModelState.AddModelError("", "Role already exists in database");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = _authManager.GetAllRolesAsync();
            return View(roles);
        }


        [HttpGet]
        public async Task<IActionResult> ListUsers()
        {
            var users = await _authManager.GetAllUsersAsync();

            foreach (var item in users)
            {
                var user = await _authManager.FindUserByIdAsync(item.Id);
                IList<string> userRoles = await _authManager.GetUserRolesAsync(user);
                item.SelectedRoles = String.Join(", ", userRoles.ToList());
            }
            return View(users);
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateUser(UserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.ImageFile != null)
                model.Image = _fileService.SaveImage(model.ImageFile).Item2;

            var result = await _authManager.RegisterAsync(model);// CreateUserAsync(model);
            
            if (result.StatusCode != 1)
            {
                TempData["ErrorMessage"] = ("User creation failed");
                TempData["msg"] = "User creation failed";
               
                    ModelState.AddModelError("", result.Message);
                
                return View(model);
            }

            TempData["SuccessMessage"] = "User created successfully!";
            return RedirectToAction("ListUsers");
        }


        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var UserViewModel = await _authManager.FindUserByIdAsync(id);

            if (UserViewModel == null)
            {
                ViewBag.ErrorMessage = $"User with ID = {id} cannot be found";
                return View("NotFound");
            }

            IList<string> userRoles = await _authManager.GetUserRolesAsync(UserViewModel);

            UserViewModel.Roles = userRoles.ToList();
            ViewBag.Roles = _authManager.GetAllRolesAsync().ToList();

            return View(UserViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserViewModel model)
        {
           
            var result = await _authManager.UpdateUserAsync(model);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "User Updated!";
            }
            else
            {
                TempData["ErrorMessage"] = ("User update failed!");
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
            return RedirectToAction("ListUsers");
        }


        [HttpPost]
        public async Task<IActionResult> UpdateUserRoles(UserViewModel model)
        {
            var user = await _authManager.FindUserByIdAsync(model.Id);

            if (user != null)
            {
                //remove all roles
                var userRoles = await _authManager.GetUserRolesAsync(user);
                IdentityResult result =  await _authManager.RemoveUserFromRolesAsync(user, userRoles.ToList());

                if (model.Roles.Count > 0)
                {
                    //add user to roles
                    result = await _authManager.AddUserToRolesAsync(user, model.Roles.ToArray());
                    if (result.Succeeded)
                    {
                        TempData["SuccessMessage"] = "User roles updated!";
                    }
                }
            }
            
            return RedirectToAction("EditUser", new { Id = model.Id });

        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string Id)
        {
            var user = await _authManager.FindUserByIdAsync(Id);

            if (user == null)
            {
                TempData["ErrorMessage"] = ($"User with ID = {Id} cannot be found");
                ViewBag.ErrorMessage = $"User with ID = {Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await _authManager.DeleteUserAsync(user);

                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "User deleted successfully!";
                    return RedirectToAction("ListUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View("ListUsers");
        }


        [HttpPost]
        public async Task<IActionResult> DeleteRole(string Id)
        {
            var role = await _authManager.FindRoleByIdAsync(Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with ID = {Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await _authManager.DeleteRoleAsync(role);

                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Role deleted successfully!";
                    return RedirectToAction("ListRoles");
                }

                foreach (var error in result.Errors)
                {
                    TempData["ErrorMessage"] = ("Role deletion failed.");
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View("ListUsers");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View("AccessDenied");
        }

    }

}
