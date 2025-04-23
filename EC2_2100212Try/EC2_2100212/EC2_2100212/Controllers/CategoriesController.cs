
using EC2_2100212.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EC2_2100212.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace EC2_2100212.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class CategoriesController : Controller
    {
        private readonly IGenericService<CategoryViewModel> _service;

        public CategoriesController(IGenericService<CategoryViewModel> service)
        {
            _service = service;
        }

        // GET: Genre
        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAllAsync());
        }

        // GET: Genre/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genre = await _service.GetByIdAsync(id);

            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        // GET: Genre/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Genre/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryViewModel model)
        {

            if (ModelState.IsValid)
            {
                await _service.InsertAsync(model);
                TempData["SuccessMessage"] = "Role created successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Error with role creation!";
            return View(model);
        }

        // GET: Genre/Edit/5
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

        // POST: Genre/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _service.UpdateAsync(model);

                    TempData["SuccessMessage"] = "Role updated successfully";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GenreExists(model.Name))
                    {

                        return View(model);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            TempData["ErrorMessage"] = "Error with editing the role!";
            return View(model);
        }

        // GET: Genre/Delete/5
        public async Task<IActionResult> Delete(int id)
        {

            var model = await _service.GetByIdAsync(id);
            return View(model);

        }

        // POST: Genre/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var genre = await _service.GetByIdAsync(id);
            if (genre != null)
            {
                await _service.DeleteAsync(id);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool GenreExists(string name)
        {
            return _service.GetAll().Result.Any(e => e.Name == name);
        }
    }
}
