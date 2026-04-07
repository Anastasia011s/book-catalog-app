using BookCatalogApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BookCatalogApp.Data
{
    public static class SeedData
    {
        public static void Initialize(AppDbContext context)
        {
            if (!context.Books.Any())
            {
                context.Books.AddRange(
                    new Book
                    {
                        Title = "1984",
                        Author = "Джордж Оруэлл",
                        Genre = "Антиутопия",
                        Year = 1949,
                        Description = "Культовый роман о тоталитарном обществе, контроле и потере свободы личности.",
                        CoverPath = "/images/1984.jpg"
                    },
                    new Book
                    {
                        Title = "Война и мир",
                        Author = "Лев Толстой",
                        Genre = "Роман",
                        Year = 1869,
                        Description = "Масштабное произведение о войне 1812 года, семье, любви и судьбе человека.",
                        CoverPath = "/images/war.jpg"
                    },
                    new Book
                    {
                        Title = "Мастер и Маргарита",
                        Author = "Михаил Булгаков",
                        Genre = "Роман, мистика",
                        Year = 1967,
                        Description = "Философский роман о добре, зле, любви и свободе.",
                        CoverPath = "/images/master.jpg"
                    },
                    new Book
                    {
                        Title = "Преступление и наказание",
                        Author = "Ф.М. Достоевский",
                        Genre = "Роман",
                        Year = 1866,
                        Description = "Психологический роман о вине, совести и нравственном выборе.",
                        CoverPath = "/images/crime.jpg"
                    },
                    new Book
                    {
                        Title = "Идиот",
                        Author = "Ф.М. Достоевский",
                        Genre = "Роман",
                        Year = 1869,
                        Description = "История князя Мышкина и столкновения добра с жестоким миром.",
                        CoverPath = "/images/idiot.jpg"
                    },
                    new Book
                    {
                        Title = "Анна Каренина",
                        Author = "Лев Толстой",
                        Genre = "Роман",
                        Year = 1877,
                        Description = "Роман о любви, обществе, семье и внутренних конфликтах человека.",
                        CoverPath = "/images/anna.jpg"
                    },
                    new Book
                    {
                        Title = "Маленький принц",
                        Author = "Антуан де Сент-Экзюпери",
                        Genre = "Сказка",
                        Year = 1943,
                        Description = "Философская сказка о дружбе, любви и взрослении.",
                        CoverPath = "/images/prince.jpg"
                    },
                    new Book
                    {
                        Title = "Гамлет",
                        Author = "Уильям Шекспир",
                        Genre = "Трагедия",
                        Year = 1603,
                        Description = "Знаменитая трагедия о долге, сомнениях и мести.",
                        CoverPath = "/images/hamlet.jpg"
                    },
                    new Book
                    {
                        Title = "Гордость и предубеждение",
                        Author = "Джейн Остин",
                        Genre = "Роман",
                        Year = 1813,
                        Description = "Классический роман о любви, характере и общественных нормах.",
                        CoverPath = "/images/pride.jpg"
                    },
                    new Book
                    {
                        Title = "Хоббит",
                        Author = "Дж. Р. Р. Толкин",
                        Genre = "Фэнтези",
                        Year = 1937,
                        Description = "Приключенческая история о Бильбо Бэггинсе и его путешествии.",
                        CoverPath = "/images/hobbit.jpg"
                    }
                );

                context.SaveChanges();
            }

            var books = context.Books.ToList();

            SetLibraryStatus(books, "Война и мир", "Прочитано", new DateTime(2025, 5, 12));
            SetLibraryStatus(books, "Мастер и Маргарита", "В процессе", new DateTime(2025, 6, 14));
            SetLibraryStatus(books, "1984", "Хочу прочитать", new DateTime(2025, 7, 2));
            SetLibraryStatus(books, "Анна Каренина", "Прочитано", new DateTime(2025, 8, 21));
            SetLibraryStatus(books, "Маленький принц", "Аудиокнига", new DateTime(2025, 9, 5));

            context.SaveChanges();

            if (!context.Reviews.Any())
            {
                var master = context.Books.FirstOrDefault(b => b.Title == "Мастер и Маргарита");
                var war = context.Books.FirstOrDefault(b => b.Title == "Война и мир");
                var nineteen = context.Books.FirstOrDefault(b => b.Title == "1984");

                if (master != null)
                {
                    context.Reviews.AddRange(
                        new Review { BookId = master.Id, ReviewerName = "Елена", ReviewText = "Булгаков — гений. Каждый раз читаю как впервые.", Rating = 5 },
                        new Review { BookId = master.Id, ReviewerName = "Дмитрий", ReviewText = "Необычная атмосфера, смесь мистики и реализма.", Rating = 5 }
                    );
                }

                if (war != null)
                {
                    context.Reviews.AddRange(
                        new Review { BookId = war.Id, ReviewerName = "Анна", ReviewText = "Книга потрясающая, прожила вместе с героями целую жизнь.", Rating = 5 },
                        new Review { BookId = war.Id, ReviewerName = "Максим", ReviewText = "Сначала тяжело, потом невозможно оторваться.", Rating = 4 }
                    );
                }

                if (nineteen != null)
                {
                    context.Reviews.Add(
                        new Review { BookId = nineteen.Id, ReviewerName = "Полина", ReviewText = "Очень сильная книга о свободе и контроле.", Rating = 5 }
                    );
                }

                context.SaveChanges();
            }
        }

        private static void SetLibraryStatus(List<Book> books, string title, string status, DateTime date)
        {
            var book = books.FirstOrDefault(b => b.Title == title);
            if (book != null && string.IsNullOrWhiteSpace(book.LibraryStatus))
            {
                book.LibraryStatus = status;
                book.AddedAt = date;
            }
        }
    }
}