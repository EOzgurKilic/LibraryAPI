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
        private AuthorsController GetControllerWithInMemoryDb(out LibraryContext context)
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            context = new LibraryContext(options);

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
            
            var controller = GetControllerWithInMemoryDb(out var context);


           
            var result = await controller.GetAuthors();

           
            var okResult = Assert.IsType<ActionResult<IEnumerable<Author>>>(result);
            var authors = Assert.IsType<List<Author>>(okResult.Value);
            Assert.Equal(2, authors.Count);
        }
        
        [Fact]
        public async Task CreateAuthor_AddsAuthor_WithExactValues()
        {
            var controller = GetControllerWithInMemoryDb(out var context);

        
            var inputAuthor = new Author { Id = 3, Name = "New Test Author" };

          
            var newAuthor = new Author { Id = inputAuthor.Id, Name = inputAuthor.Name };

            var result = await controller.CreateAuthor(newAuthor);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedAuthor = Assert.IsType<Author>(createdAtActionResult.Value);

          
            Assert.Equal(inputAuthor.Id, returnedAuthor.Id);
            Assert.Equal(inputAuthor.Name, returnedAuthor.Name);

            
            var authorInDb = await context.Authors
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == inputAuthor.Id);

            Assert.NotNull(authorInDb);
            Assert.Equal(inputAuthor.Id, authorInDb.Id);        
            Assert.Equal(inputAuthor.Name, authorInDb.Name);     
        }


        
        [Fact]
        public async Task UpdateAuthor_ValidInput_EnforcesExactMatch()
        {
            var controller = GetControllerWithInMemoryDb(out var context);
            var updatedAuthor = new Author { Id = 1, Name = "Updated Author Name" };

            var result = await controller.UpdateAuthor(1, updatedAuthor);
            Assert.IsType<NoContentResult>(result);

            var authorInDb = await context.Authors.AsNoTracking().FirstOrDefaultAsync(a => a.Id == updatedAuthor.Id);
            Assert.NotNull(authorInDb);
            Assert.Equal(updatedAuthor.Id, authorInDb.Id);
            Assert.Equal(updatedAuthor.Name, authorInDb.Name);
        }
        
        [Fact]
        public async Task DeleteAuthor_RemovesCorrectAuthor()
        {
            var controller = GetControllerWithInMemoryDb(out var context);
            var authorIdToDelete = 2;

            var result = await controller.DeleteAuthor(authorIdToDelete);
            Assert.IsType<NoContentResult>(result);

            var deletedAuthor = await context.Authors.FindAsync(authorIdToDelete);
            Assert.Null(deletedAuthor);

            var remainingAuthor = await context.Authors.FindAsync(1);
            Assert.NotNull(remainingAuthor);
            Assert.NotEqual(authorIdToDelete, remainingAuthor.Id);
        }

        
    }
}
