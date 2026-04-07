using BookCatalogApp.Data;
using BookCatalogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookCatalogApp.Controllers
{
    public class BooksController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public BooksController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> Index(
            string? genre,
            string? author,
            int? yearFrom,
            int? yearTo,
            string? title)
        {
            var query = _context.Books.AsQueryable();

            if (!string.IsNullOrWhiteSpace(genre))
                query = query.Where(b => b.Genre == genre);

            if (!string.IsNullOrWhiteSpace(author))
                query = query.Where(b => b.Author == author);

            if (yearFrom.HasValue)
                query = query.Where(b => b.Year >= yearFrom.Value);

            if (yearTo.HasValue)
                query = query.Where(b => b.Year <= yearTo.Value);

            ViewBag.Genres = await _context.Books
                .Select(b => b.Genre)
                .Distinct()
                .OrderBy(g => g)
                .ToListAsync();

            ViewBag.Authors = await _context.Books
                .Select(b => b.Author)
                .Distinct()
                .OrderBy(a => a)
                .ToListAsync();

            ViewBag.SelectedGenre = genre;
            ViewBag.SelectedAuthor = author;
            ViewBag.YearFrom = yearFrom;
            ViewBag.YearTo = yearTo;
            ViewBag.TitleQuery = title;

            var books = await query.ToListAsync();

            if (!string.IsNullOrWhiteSpace(title))
            {
                var search = title.Trim();
                books = books
                    .Where(b => !string.IsNullOrWhiteSpace(b.Title) &&
                                b.Title.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return View(books);
        }

        public async Task<IActionResult> Details(int id)
        {
            var book = await _context.Books
                .Include(b => b.Reviews)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
                return NotFound();

            book.Reviews = book.Reviews
                .OrderByDescending(r => r.CreatedAt)
                .ToList();

            return View(book);
        }

        [HttpGet]
        public async Task<IActionResult> GetRandomBook(int? excludeId)
        {
            var query = _context.Books.AsQueryable();

            if (excludeId.HasValue)
                query = query.Where(b => b.Id != excludeId.Value);

            var count = await query.CountAsync();
            if (count == 0)
                return NotFound();

            var rnd = new Random();
            var book = await query.Skip(rnd.Next(count)).FirstAsync();

            return Json(new
            {
                id = book.Id,
                title = book.Title,
                author = book.Author,
                year = book.Year,
                cover = book.CoverPath,
                href = Url.Action("Details", "Books", new { id = book.Id })
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToLibrary(int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book == null)
                return NotFound();

            if (string.IsNullOrWhiteSpace(book.LibraryStatus))
            {
                book.LibraryStatus = "Хочу прочитать";
                book.AddedAt = DateTime.Today;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview(int id, string reviewerName, string reviewText)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book == null)
                return NotFound();

            if (!string.IsNullOrWhiteSpace(reviewerName) && !string.IsNullOrWhiteSpace(reviewText))
            {
                _context.Reviews.Add(new Review
                {
                    BookId = id,
                    ReviewerName = reviewerName.Trim(),
                    ReviewText = reviewText.Trim(),
                    Rating = 5,
                    CreatedAt = DateTime.Now
                });

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book, IFormFile? coverFile)
        {
            if (string.IsNullOrWhiteSpace(book.Title) || book.Title.Trim().Length < 3)
                ModelState.AddModelError("Title", "Название книги должно содержать минимум 3 символа");

            if (string.IsNullOrWhiteSpace(book.Author))
                ModelState.AddModelError("Author", "Введите автора");

            if (string.IsNullOrWhiteSpace(book.Genre))
                ModelState.AddModelError("Genre", "Введите жанр");

            if (book.Year < 1500 || book.Year > 2026)
                ModelState.AddModelError("Year", "Год должен быть в диапазоне от 1500 до 2026");

            if (coverFile == null || coverFile.Length == 0)
                ModelState.AddModelError("CoverPath", "Загрузите обложку книги");

            if (!ModelState.IsValid)
                return View(book);

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid() + Path.GetExtension(coverFile.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await coverFile.CopyToAsync(stream);
            }

            book.CoverPath = "/images/" + uniqueFileName;

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Книга успешно добавлена";
            return RedirectToAction(nameof(Create));
        }
    }
}