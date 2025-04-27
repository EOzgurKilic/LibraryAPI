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
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
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
            
            var controller = GetControllerWithInMemoryDb();

           
            var result = await controller.GetAuthors();

           
            var okResult = Assert.IsType<ActionResult<IEnumerable<Author>>>(result);
            var authors = Assert.IsType<List<Author>>(okResult.Value);
            Assert.Equal(2, authors.Count);
        }
        
        [Fact]
        public async Task CreateAuthor_AddsAuthor()
        {
           
            var controller = GetControllerWithInMemoryDb();
            var newAuthor = new Author { Name = "New Test Author" };

           
            var result = await controller.CreateAuthor(newAuthor);

           
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var author = Assert.IsType<Author>(createdAtActionResult.Value);
            Assert.Equal("New Test Author", author.Name);
        }
        
        [Fact]
        public async Task UpdateAuthor_ValidUpdate()
        {
            
            var controller = GetControllerWithInMemoryDb();
            var updatedAuthor = new Author { Id = 1, Name = "Updated Author Name" };

            
            var result = await controller.UpdateAuthor(1, updatedAuthor);

           
            Assert.IsType<NoContentResult>(result);
        }
        
        [Fact]
        public async Task DeleteAuthor_RemovesAuthor()
        {
           
            var controller = GetControllerWithInMemoryDb();

           
            var result = await controller.DeleteAuthor(1);

           
            Assert.IsType<NoContentResult>(result);
        }

        
    }
}
