using LibraryAPI.Controllers;
using LibraryAPI.Data;
using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

using Xunit;

namespace LibraryAPI.Tests.ControllersTests
{
    public class BooksControllerTests
    {
        private BooksController GetControllerWithInMemoryDb()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())  // Benzersiz DB ismi
                .Options;

            var context = new LibraryContext(options);

            // ⭐ Önce Author ekle (Çünkü Book eklemek için Author gerekiyor)
            context.Authors.Add(new Author { Id = 1, Name = "Test Author" });
            context.SaveChanges();

            // Sonra Book ekleyebilirsin (Eğer test için gerekiyorsa)
            context.Books.AddRange(
                new Book { Id = 1, Title = "Test Book 1", AuthorId = 1 },
                new Book { Id = 2, Title = "Test Book 2", AuthorId = 1 }
            );
            context.SaveChanges();

            return new BooksController(context);
        }



        [Fact]
        public async Task GetBooks_ReturnsAllBooks()
        {
            // Arrange
            var controller = GetControllerWithInMemoryDb();

            // Act
            var actionResult = await controller.GetBooks();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);  // Ensure it's 200 OK
            var books = Assert.IsAssignableFrom<IEnumerable<object>>(okResult.Value);  // Flexible type check
            Assert.Equal(2, books.Count());
        }

        [Fact]
        public async Task CreateBook_AddsBook()
        {
            // Arrange
            var controller = GetControllerWithInMemoryDb();
            var newBook = new Book { Id = 3, Title = "New Test Book", AuthorId = 1 };

            // Act
            var result = await controller.CreateBook(newBook);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedBook = Assert.IsType<Book>(createdAtActionResult.Value);
            Assert.Equal("New Test Book", returnedBook.Title);
        }

        [Fact]
        public async Task UpdateBook_ValidUpdate()
        {
            // Arrange
            var controller = GetControllerWithInMemoryDb();
            var updatedBook = new Book { Id = 1, Title = "Updated Title", AuthorId = 1 };

            // Act
            var result = await controller.UpdateBook(1, updatedBook);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteBook_RemovesBook()
        {
            // Arrange
            var controller = GetControllerWithInMemoryDb();

            // Act
            var result = await controller.DeleteBook(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

    }
}