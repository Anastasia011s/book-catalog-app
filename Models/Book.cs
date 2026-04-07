using System.ComponentModel.DataAnnotations;

namespace BookCatalogApp.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите название книги")]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; } = "";

        [Required(ErrorMessage = "Введите автора")]
        [StringLength(100)]
        public string Author { get; set; } = "";

        [Required(ErrorMessage = "Введите жанр")]
        [StringLength(50)]
        public string Genre { get; set; } = "";

        [Range(1500, 2026, ErrorMessage = "Введите корректный год")]
        public int Year { get; set; }

        [StringLength(2000)]
        public string Description { get; set; } = "";

        public string? CoverPath { get; set; }

        public string? LibraryStatus { get; set; }
        public DateTime? AddedAt { get; set; }

        public List<Review> Reviews { get; set; } = new();
    }
}