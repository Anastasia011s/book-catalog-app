using BookCatalogApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookCatalogApp.Controllers
{
    public class LibraryController : Controller
    {
        private readonly AppDbContext _context;

        public LibraryController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? status)
        {
            var query = _context.Books
                .Where(b => !string.IsNullOrWhiteSpace(b.LibraryStatus))
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(b => b.LibraryStatus == status);

            ViewBag.SelectedStatus = status ?? "Все";

            var books = await query
                .OrderBy(b => b.AddedAt)
                .ToListAsync();

            return View(books);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book == null) return NotFound();

            book.LibraryStatus = null;
            book.AddedAt = null;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}