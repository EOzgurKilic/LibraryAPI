using Xunit;
using Moq;
using LibraryAPI.Controllers;
using LibraryAPI.Data;
using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace LibraryAPI.Tests.ControllersTests
{
    public class AuthorsControllerTests
    {
        private AuthorsController GetControllerWithInMemoryDb()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())  // Benzersiz DB adı
                .Options;

            var context = new LibraryContext(options);

            context.Authors.AddRange(
                new Author { Id = 1, Name = "Test Author 1" },
                new Author { Id = 2, Name = "Test Author 2" }
            );
            context.SaveChanges();

            return new AuthorsController(context);
        }


        [Fact]
        public async Task GetAuthors_ReturnsAllAuthors()
        {
            // Arrange
            var controller = GetControllerWithInMemoryDb();

            // Act
            var result = await controller.GetAuthors();

            // Assert
            var okResult = Assert.IsType<ActionResult<IEnumerable<Author>>>(result);
            var authors = Assert.IsType<List<Author>>(okResult.Value);
            Assert.Equal(2, authors.Count);
        }
        
        [Fact]
        public async Task CreateAuthor_AddsAuthor()
        {
            // Arrange
            var controller = GetControllerWithInMemoryDb();
            var newAuthor = new Author { Name = "New Test Author" };

            // Act
            var result = await controller.CreateAuthor(newAuthor);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var author = Assert.IsType<Author>(createdAtActionResult.Value);
            Assert.Equal("New Test Author", author.Name);
        }
        
        [Fact]
        public async Task UpdateAuthor_ValidUpdate()
        {
            // Arrange
            var controller = GetControllerWithInMemoryDb();
            var updatedAuthor = new Author { Id = 1, Name = "Updated Author Name" };

            // Act
            var result = await controller.UpdateAuthor(1, updatedAuthor);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
        
        [Fact]
        public async Task DeleteAuthor_RemovesAuthor()
        {
            // Arrange
            var controller = GetControllerWithInMemoryDb();

            // Act
            var result = await controller.DeleteAuthor(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        
    }
}
