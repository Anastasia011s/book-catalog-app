using System.ComponentModel.DataAnnotations;

namespace BookCatalogApp.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        public int BookId { get; set; }

        public Book? Book { get; set; }

        [Required(ErrorMessage = "Введите имя")]
        [StringLength(100)]
        public string ReviewerName { get; set; } = "";

        [Required(ErrorMessage = "Введите текст отзыва")]
        [StringLength(1000)]
        public string ReviewText { get; set; } = "";

        [Range(1, 5)]
        public int Rating { get; set; } = 5;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}