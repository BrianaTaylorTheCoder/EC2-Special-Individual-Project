using Microsoft.AspNetCore.Mvc;
using EC2_2100212.ViewModels;
using EC2_2100212.Services.Interfaces;
using EC2_2100212.Web.Interfaces;


namespace EC2_2100212.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthManager _authManager;
        private readonly IFileService _fileService;

        public AccountController(IAuthManager authManager, IFileService fileService)
        {
            _authManager = authManager;
            _fileService = fileService;
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = "null")
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _authManager.LoginAsync(model);
            if (result.StatusCode == 0)
            {
                TempData["ErrorMessage"] = result.Message;
                return View(model);
            }
            TempData["SuccessMessage"] = "Login Successful!";

            // Redirect to the original page requested or default to Home
            if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
            {
                return Redirect(returnUrl); // Redirect to the original URL
            }
            else
            {
                return RedirectToAction("Index", "Home"); // Default redirect to home page
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string? returnURL)
        {
            await _authManager.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserViewModel model)
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
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
