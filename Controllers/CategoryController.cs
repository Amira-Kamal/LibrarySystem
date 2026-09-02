using LibrarySystem.Models;
using LibrarySystem.Repositories.Interfaces;
using LibrarySystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // GET: /Category
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetAllAsync();

            return View(categories);
        }

        // GET: /Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryRepository.GetDetailsAsync(id.Value);

            if (category == null)
            {
                return NotFound();
            }

            var viewModel = new CategoryDetailsViewModel
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                Books = category.Books.ToList()
            };

            return View(viewModel);
        }

        // GET: /Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            await _categoryRepository.AddAsync(category);

            return RedirectToAction(nameof(Index));
        }

        // GET: /Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryRepository.GetByIdAsync(id.Value);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: /Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("CategoryId,Name")] Category category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(category);
            }

            if (!await _categoryRepository.ExistsAsync(category.CategoryId))
            {
                return NotFound();
            }

            await _categoryRepository.UpdateAsync(category);

            return RedirectToAction(nameof(Index));
        }

        // GET: /Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryRepository.GetByIdAsync(id.Value);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: /Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!await _categoryRepository.ExistsAsync(id))
            {
                return NotFound();
            }

            await _categoryRepository.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}