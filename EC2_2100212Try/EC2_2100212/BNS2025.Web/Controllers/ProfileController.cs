using AutoMapper;
using BNS2025.ViewModels;
using Microsoft.AspNetCore.Mvc;
using BNS2025.Services.Interfaces;

namespace BNS2025.Web.Controllers
{
    public class ProfileController : Controller
    {
     
        private readonly IMapper _mapper;
        private readonly IAuthManager _authManager;

        public ProfileController(IMapper mapper, IAuthManager authManager)
        {
            _mapper = mapper;
            _authManager = authManager;
        }
       
        
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _authManager.LoginAsync(model);
            if (result.StatusCode == 0)
            {
                ViewBag.Message = result.Message;
                return View(model);
            }
            return RedirectToAction("Index", "Home");
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

            var result = await _authManager.RegisterAsync(model);
            if (result.StatusCode == 0)
            {
                TempData["msg"] = result.Message;
                return View(model);
            }
            return RedirectToAction("Login", "Profile");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return RedirectToAction("AccessDenied", "Administrator");
        }

    }
}
