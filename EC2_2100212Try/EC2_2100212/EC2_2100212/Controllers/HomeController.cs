using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EC2_2100212.Models;
using EC2_2100212.Services.Interfaces;
using EC2_2100212.ViewModels;

namespace EC2_2100212.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IGenericService<BagViewModel> _bagService;
       
        private readonly IGenericService<OrderViewModel> _orderService;
        private readonly IGenericService<CategoryViewModel> _categoryService;
        private readonly IGenericService<BagCategoryViewModel> _bagCategoryService;
        private readonly IGenericService<BagMaterialViewModel> _bagMaterialService;
        private readonly IGenericService<MaterialViewModel> _materialService;
        private readonly UserManager<ApplicationUser> _userManager;
        
        public HomeController(ILogger<HomeController> logger, IGenericService<BagViewModel> bagService, IGenericService<OrderViewModel> orderService, IGenericService<CategoryViewModel> categoryService, IGenericService<BagCategoryViewModel> bagCategoryService, IGenericService<BagMaterialViewModel> bagMaterialService, IGenericService<MaterialViewModel> materialService, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _bagService = bagService;
            _orderService = orderService;
            _categoryService = categoryService;
            _bagCategoryService = bagCategoryService;
            _bagMaterialService = bagMaterialService;
            _materialService = materialService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var viewModelList = await _bagService.GetAllAsync();
            return View(viewModelList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("/StatusCodeError/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            if (statusCode == 404)
            {
                ViewBag.ErrorMessage = "404 Page Not Found Exception!";
            }
            return View("ErrorWithMessage");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> BuyNow(int id)
        {
            OrderViewModel viewModel = new OrderViewModel();

            viewModel.UserId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            viewModel.Bag = await _bagService.GetByIdAsync(id);

            var categoryList = _categoryService.GetAllAsync().Result;
            var bagCategorySelectedList = _bagCategoryService.GetAllAsync().Result;
            var bagMaterialSelectedList = _bagMaterialService.GetAllAsync().Result;
            var materialList = _materialService.GetAllAsync().Result;

            var categories = (from category in categoryList
                          join bc in bagCategorySelectedList
                          on category.Id equals bc.CategoryId
                          where bc.BagId == id
                          select category.Name).ToList();

            var materials = (from material in materialList
                          join bm in bagMaterialSelectedList
                          on material.Id equals bm.MaterialId
                          where bm.BagId == id
                          select material.Name).ToList();

            viewModel.Bag.Categories = String.Join(", ", categories);
            viewModel.Bag.Materials = String.Join(", ", materials);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> BuyNow(int id, OrderViewModel model)
        {

            model.Bag = await _bagService.GetByIdAsync(id);
            if (ModelState.IsValid)
            {
                model.BagId = id;
                model.Id = 0;

                if (model.Bag.Quantity >= model.Quantity)
                {
                    model.Total = model.Bag.Price * model.Quantity;
                    model.TransactionDate = DateTime.Now;
                    //withdraw money from customers account
                    using (var httpclient = new HttpClient())
                    {
                        var content = new MultipartFormDataContent();
                        content.Add(new StringContent(model.CardNumber), "CardNumber");
                        content.Add(new StringContent(model.CardExpirationDate), "CardExpirationDate");
                        content.Add(new StringContent(model.CardSecurityCode), "CardSecurityCode");
                        content.Add(new StringContent(model.Total.ToString()), "Amount");
                        content.Add(new StringContent("Bag.WebApp"), "Source");
                        content.Add(new StringContent("1"), "TransactionType"); //withdrawal
                        content.Add(new StringContent(model.UserId.ToString()), "UserId");
                        content.Add(new StringContent(model.TransactionDate.ToString()), "TransactionDate");

                 
                        using (var response = await httpclient.PutAsync("https://localhost:7099/api/Account/Withdrawal/", content))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            if (apiResponse == "true")
                            {
                                ViewBag.Result = "Success";
                            }
                            else
                            {
                                ViewBag.Result = "Failure!" + response.ToString();
                                return View(model);
                            }
                        }
                    }

                    //deposit money in companies account
                    using (var httpclient = new HttpClient())
                    {
                        var content = new MultipartFormDataContent();
                        content.Add(new StringContent("111111111"), "CardNumber");
                        content.Add(new StringContent("1111"), "CardExpirationDate");
                        content.Add(new StringContent("111"), "CardSecurityCode");
                        content.Add(new StringContent(model.Total.ToString()), "Amount");
                        content.Add(new StringContent("Bag.WebApp"), "Source");
                        content.Add(new StringContent("0"), "TransactionType"); //deposit
                        content.Add(new StringContent(model.UserId.ToString()), "UserId");
                        content.Add(new StringContent(model.TransactionDate.ToString()), "TransactionDate");

                        using (var response = await httpclient.PutAsync("https://localhost:7099/api/Account/Deposit/", content))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            if (apiResponse == "true")
                            {
                                ViewBag.Result = "Success";
                            }
                            else
                            {
                                ViewBag.Result = "Failure!" + response.ToString();
                                return View(model);
                            }
                        }
                    }

                    await _orderService.InsertAsync(model);

                    model.Bag.Quantity -= model.Quantity;
                    await _bagService.UpdateAsync(model.Bag);
                }
                else
                {
                    ModelState.AddModelError("1", "Quantity Too Low");
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("2", "Model State not valid");
                return View(model);
            }
            return RedirectToAction("Index");
        }
    }
}
