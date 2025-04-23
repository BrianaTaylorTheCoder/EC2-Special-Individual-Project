using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EC2_2100212.ViewModels;
using EC2_2100212.Services.Interfaces;
using EC2_2100212.Web.Interfaces;
using EC2_2100212.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace EC2_2100212.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class MaterialsController : Controller
    {
        private readonly IGenericService<MaterialViewModel> _service;
        private readonly IFileService _fileService;

        public MaterialsController(IGenericService<MaterialViewModel> service, IFileService fileService)
        {
            _service = service;
            _fileService = fileService;
        }

        // GET: Material
        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAllAsync());
        }

        // GET: Material/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var viewModel = await _service.GetByIdAsync(id);

            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        // GET: Material/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Material/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MaterialViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.ImageFile != null)
                {
                    model.Image = _fileService.SaveImage(model.ImageFile).Item2;
                }
                await _service.InsertAsync(model);
                TempData["SuccessMessage"] = "Material created successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Material/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _service.GetByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: Material/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MaterialViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (model.ImageFile != null)
                    {
                        if (model.Image != null) //if there was already an image
                        {
                            _fileService.DeleteImage(model.Image);
                        }
                        model.Image = _fileService.SaveImage(model.ImageFile).Item2;
                    }
                    await _service.UpdateAsync(model);
                    TempData["SuccessMessage"] = "Material updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


        // GET: Material/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _service.GetByIdAsync(id);
            return View(model);
        }

        // POST: Material/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var model = await _service.GetByIdAsync(id);
            if (model != null)
            {
                await _service.DeleteAsync(id);
                TempData["SuccessMessage"] = "Material deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
