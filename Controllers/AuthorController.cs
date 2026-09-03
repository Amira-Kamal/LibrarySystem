using LibrarySystem.Models;
using LibrarySystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace LibrarySystem.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        // GET: /Author
        [HttpGet]
        public IActionResult Index()
        {
            var authors = _authorRepository.GetAll();
            return View(authors);
        }

        // GET: /Author/Details/{id}
        [HttpGet]
        public IActionResult Details(int id)
        {
            var author = _authorRepository.GetById(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        // GET: /Author/Create
        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Author/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(Author author)
        {
            if (ModelState.IsValid)
            {
                _authorRepository.Add(author);
                _authorRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // GET: /Author/Edit/{id}
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var author = _authorRepository.GetById(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        // POST: /Author/Edit
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Author author)
        {
            if (ModelState.IsValid)
            {
                _authorRepository.Update(author);
                _authorRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // GET: /Author/Delete/{id}
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var author = _authorRepository.GetById(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        // POST: /Author/Delete
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var author = _authorRepository.GetById(id);
            if (author != null)
            {
                _authorRepository.Delete(id);
                _authorRepository.Save();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}