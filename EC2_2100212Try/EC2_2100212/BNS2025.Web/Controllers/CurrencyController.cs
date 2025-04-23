using AutoMapper;
using BNS2025.ViewModels;
using BNS2025.Models;
using BNS2025.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BNS2025.Web.Controllers
{

    [Authorize(Roles = "Administrator")]
    [Authorize(Roles = "Teller")]
    public class CurrencyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CurrencyController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var currencies =  await _unitOfWork.GenericRepository<Currency>().GetAllAsync();
            var results = _mapper.Map<IList<CurrencyViewModel>>(currencies);

            return View(results);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CurrencyViewModel model)
        {
            var currency = _mapper.Map<Currency>(model);
            await _unitOfWork.GenericRepository<Currency>().InsertAsync(currency);
            await _unitOfWork.Save();

            TempData["SuccessMessage"] = "Currency created successfully!";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var currency = await _unitOfWork.GenericRepository<Currency>().GetByIdAsync(id);
            var viewModel = _mapper.Map<CurrencyViewModel>(currency);

            return View(viewModel);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CurrencyViewModel model)
        {
            var currency = _mapper.Map<Currency>(model);
            await _unitOfWork.GenericRepository<Currency>().UpdateAsync(currency);
            await _unitOfWork.Save();

            TempData["SuccessMessage"] = "Currency updated successfully!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var currencies = await _unitOfWork.GenericRepository<Currency>().GetByIdAsync(id);
            var viewModel = _mapper.Map<CurrencyViewModel>(currencies);

            return View(viewModel);

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, CurrencyViewModel model)
        {
            await _unitOfWork.GenericRepository<Currency>().DeleteAsync(id);
            await _unitOfWork.Save();

            TempData["SuccessMessage"] = "Currency deleted successfully!";

            return RedirectToAction("Index");

        }
    }
}
