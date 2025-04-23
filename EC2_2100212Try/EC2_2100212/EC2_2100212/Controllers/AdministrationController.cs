using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EC2_2100212.Web.Interfaces;
using EC2_2100212.ViewModels;
using EC2_2100212.Services.Interfaces;

namespace EC2_2100212.Web.Controllers
{

    [Authorize(Roles = "Administrator")]
    public class AdministrationController : Controller
    {
        private readonly IAuthManager _authManager;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public AdministrationController(IAuthManager authManager, IMapper mapper, IFileService fileService)
        {
            _authManager = authManager;
            _mapper = mapper;
            _fileService = fileService;
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (model.Name != null)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.Name
                };

                IdentityResult res = await _authManager.CreateRoleAsync(identityRole);

                if (res.Succeeded)
                {
                    TempData["SuccessMessage"] = "Role Created!";
                    return RedirectToAction("ListRoles", "Administration");
                }

                foreach (var error in res.Errors)
                {
                    ModelState.AddModelError("", error.Description);
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
                ViewBag.ErrorMessage = $"Role with ID = {Id} cannor be found";
                return View("NotFound");
            }
            var model = _mapper.Map<RoleViewModel>(role);

            var users = await _authManager.GetAllUsersAsync();
            model.Users = new List<UserViewModel>();
            //adds to the list of users assigned to this role
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
                        IdentityResult res = await _authManager.UpdateRoleAsync(role);

                        if (res.Succeeded)
                        {
                            TempData["SuccessMessage"] = "Role updated!";
                            return RedirectToAction("ListRoles", "Administration");
                        }

                        foreach (var error in res.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                    ModelState.AddModelError("", "Role Id Does Not Exists in Database");
                }
                ModelState.AddModelError("", "Role Already Exists in Database");
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
                item.Roles = String.Join(", ", userRoles.ToList());
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

            var result = await _authManager.RegisterAsync(model);
            if (result.StatusCode == 0)
            {
                TempData["ErrorMessage"] = result.Message;
                return View(model);
            }

            TempData["SuccessMessage"] = "User created!";
            return RedirectToAction("ListUsers");
        }


        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var UserViewModel = await _authManager.FindUserByIdAsync(id);

            if (UserViewModel == null)
            {

                ViewBag.ErrorMessage = $"User with ID = {id} cannor be found";
                return View("NotFound");
            }
            var userRoles = await _authManager.GetUserRolesAsync(UserViewModel);

            UserViewModel.RolesCollection = userRoles.ToList();
            ViewBag.Roles = _authManager.GetAllRolesAsync().ToList();

            return View(UserViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserViewModel model)
        {
            if (model.ImageFile != null)
                model.Image = _fileService.SaveImage(model.ImageFile).Item2;

            var result = await _authManager.UpdateUserAsync(model);
            TempData["SuccessMessage"] = "User Updated!";
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
                IdentityResult result = await _authManager.RemoveUserFromRolesAsync(user, userRoles.ToList());

                if (model.RolesCollection.Count() > 0)
                {
                    //add user to roles
                    result = await _authManager.AddUserToRolesAsync(user, model.RolesCollection.ToList());
                }
            }

            TempData["SuccessMessage"] = "User Roles Updated!";
            return RedirectToAction("EditUser", new { Id = model.Id });

        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string Id)
        {
            var user = await _authManager.FindUserByIdAsync(Id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with ID = {Id} cannor be found";
                return View("NotFound");
            }
            else
            {
                var result = await _authManager.DeleteUserAsync(user);

                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "User Deleted!";
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
                ViewBag.ErrorMessage = $"Role with ID = {Id} cannor be found";
                return View("NotFound");
            }
            else
            {
                var result = await _authManager.DeleteRoleAsync(role);

                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Role Deleted!";
                    return RedirectToAction("ListRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View("ListRoles");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View("AccessDenied");
        }

    }
}
