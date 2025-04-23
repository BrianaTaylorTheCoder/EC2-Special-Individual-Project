using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using EC2_2100212.Web.Interfaces;
using EC2_2100212.Services.Interfaces;
using EC2_2100212.ViewModels;


namespace EC2_2100212.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class BagController : Controller
    {
        private readonly IGenericService<BagViewModel> _bagService;
        private readonly IGenericService<CategoryViewModel> _categoryService;
        private readonly IGenericService<MaterialViewModel> _materialService;
        private readonly IGenericService<BagCategoryViewModel> _bagCategoryService;
        private readonly IGenericService<BagMaterialViewModel> _bagMaterialService;
        private readonly IFileService _fileService;

        public BagController(
            IGenericService<BagViewModel> bagService,
            IGenericService<CategoryViewModel> categoryService,
            IGenericService<MaterialViewModel> materialService,
            IGenericService<BagCategoryViewModel> bagCategoryService,
            IGenericService<BagMaterialViewModel> bagMaterialService,
            IFileService fileService)
        {
            _bagService = bagService;
            _categoryService = categoryService;
            _materialService = materialService;
            _bagCategoryService = bagCategoryService;
            _bagMaterialService = bagMaterialService;
            _fileService = fileService;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var bagList = await _bagService.GetAllAsync();
            foreach (var bag in bagList)
            {
                await SetBagCategoriesAndMaterials(bag.Id, bag);
            }

            return View(bagList);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var viewModel = await _bagService.GetByIdAsync(id);
            if (viewModel == null) return NotFound();

            await SetBagCategoriesAndMaterials(id, viewModel);
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new BagViewModel
            {
                ListOfCategories = new MultiSelectList(await _categoryService.GetAllAsync(), "Id", "Name"),
                ListOfMaterials = new MultiSelectList(await _materialService.GetAllAsync(), "Id", "Name")
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BagViewModel _model)
        {
            if (_model.ImageFile != null)
                _model.Image = _fileService.SaveImage(_model.ImageFile).Item2;


            await _bagService.InsertAsync( _model);

            _model.Id = (await _bagService.GetAllAsync()).Last().Id;  // Retrieve the new Id from db

            await AddBagCategoriesAndMaterials(_model);

            TempData["AlertMsg"] = "Bag created successfully!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var viewModel = await _bagService.GetByIdAsync(id);
            if (viewModel == null) return NotFound();

            var categoryList = await _categoryService.GetAllAsync();
            var materialList = await _materialService.GetAllAsync();

            var bagCategories = await _bagCategoryService.GetAllAsync();
            var bagMaterials = await _bagMaterialService.GetAllAsync();

            viewModel.CategorySelectedList = bagCategories
                .Where(bc => bc.BagId == id)
                .Select(bc => bc.CategoryId)
                .ToList();

            viewModel.MaterialSelectedList = bagMaterials
                .Where(bm => bm.BagId == id)
                .Select(bm => bm.MaterialId)
                .ToList();

            viewModel.ListOfCategories = new MultiSelectList(categoryList, "Id", "Name");
            viewModel.ListOfMaterials = new MultiSelectList(materialList, "Id", "Name");

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, BagViewModel _model)
        {
            if (_model.ImageFile != null)
            {
                if (_model.Image != null) _fileService.DeleteImage(_model.Image);
                _model.Image = _fileService.SaveImage(_model.ImageFile).Item2;
            }

            await _bagService.UpdateAsync(_model);

            await RemoveExistingCategoriesAndMaterials(id);
            await AddBagCategoriesAndMaterials(_model);

            TempData["AlertMsg"] = "Bag updated successfully!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var viewModel = await _bagService.GetByIdAsync(id);
            if (viewModel == null) return NotFound();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, BagViewModel _model)
        {
            if (_model.Image != null)
                _fileService.DeleteImage(_model.Image);

            await RemoveExistingCategoriesAndMaterials(id);
            await _bagService.DeleteAsync(id);

            TempData["AlertMsg"] = "Bag Deleted successfully!";
            return RedirectToAction("Index");
        }


        private async Task SetBagCategoriesAndMaterials(int bagId, BagViewModel _model)
        {
            var categoryList = await _categoryService.GetAllAsync();
            var materialList = await _materialService.GetAllAsync();
            var bagCategories = await _bagCategoryService.GetAllAsync();
            var bagMaterials = await _bagMaterialService.GetAllAsync();

            _model.Categories = string.Join(", ", categoryList
                .Where(category => bagCategories.Any(bc => bc.BagId == bagId && bc.CategoryId == category.Id))
                .Select(category => category.Name));

            _model.Materials = string.Join(", ", materialList
                .Where(material => bagMaterials.Any(bm => bm.BagId == bagId && bm.MaterialId == material.Id))
                .Select(material => material.Name));
        }

        private async Task RemoveExistingCategoriesAndMaterials(int bagId)
        {
            var bagCategories = await _bagCategoryService.GetAllAsync();
            var bagMaterials = await _bagMaterialService.GetAllAsync();

            foreach (var bc in bagCategories.Where(bc => bc.BagId == bagId))
                await _bagCategoryService.DeleteAsync(bc.Id);

            foreach (var bm in bagMaterials.Where(bm => bm.BagId == bagId))
                await _bagMaterialService.DeleteAsync(bm.Id);
        }

        private async Task AddBagCategoriesAndMaterials(BagViewModel _model)
        {
            if (_model.CategorySelectedList != null)
            {
                foreach (var categoryId in _model.CategorySelectedList)
                {
                    await _bagCategoryService.InsertAsync(new BagCategoryViewModel { BagId = _model.Id, CategoryId = categoryId });
                }
            }
            if (_model.MaterialSelectedList != null)
            {
                foreach (var materialId in _model.MaterialSelectedList)
                {
                    await _bagMaterialService.InsertAsync(new BagMaterialViewModel { BagId = _model.Id, MaterialId = materialId });
                }
            }
        }
    }
}

