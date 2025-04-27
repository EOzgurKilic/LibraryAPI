using LibraryAPI.Models;

namespace LibraryAPI.Data;

public static class DataSeeder
{
    public static void Seed(LibraryContext context)
    {
        if (!context.Authors.Any())
        {
            var author1 = new Author { Name = "George Orwell" };
            var author2 = new Author { Name = "Jane Austen" };

            var book1 = new Book { Title = "1984", Author = author1 };
            var book2 = new Book { Title = "Animal Farm", Author = author1 };
            var book3 = new Book { Title = "Pride and Prejudice", Author = author2 };

            context.Authors.AddRange(author1, author2);
            context.Books.AddRange(book1, book2, book3);

            context.SaveChanges();
        }
    }
}