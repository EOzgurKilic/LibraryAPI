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
        private BooksController GetControllerWithInMemoryDb(out LibraryContext context)
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            context = new LibraryContext(options);

            context.Authors.Add(new Author { Id = 1, Name = "Test Author" });
            context.SaveChanges();

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
            var controller = GetControllerWithInMemoryDb(out _);

            var actionResult = await controller.GetBooks();
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var books = Assert.IsAssignableFrom<IEnumerable<object>>(okResult.Value);
            Assert.Equal(2, books.Count());
        }

        [Fact]
        public async Task CreateBook_AddsBook_WithExactValues()
        {
            var controller = GetControllerWithInMemoryDb(out var context);
            var originalBook = new Book { Id = 3, Title = "New Test Book", AuthorId = 1 };

          
            var expectedId = originalBook.Id;
            var expectedTitle = originalBook.Title;
            var expectedAuthorId = originalBook.AuthorId;

            var result = await controller.CreateBook(originalBook);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedBook = Assert.IsType<Book>(createdAtActionResult.Value);

      
            Assert.Equal(expectedId, returnedBook.Id);
            Assert.Equal(expectedTitle, returnedBook.Title);
            Assert.Equal(expectedAuthorId, returnedBook.AuthorId);

           
            var bookInDb = await context.Books
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == expectedId);

            Assert.NotNull(bookInDb);
            Assert.Equal(expectedId, bookInDb.Id);              
            Assert.Equal(expectedTitle, bookInDb.Title);        
            Assert.Equal(expectedAuthorId, bookInDb.AuthorId);  
        }


        
        [Fact]
        public async Task UpdateBook_ValidInput_EnforcesExactMatch()
        {
            var controller = GetControllerWithInMemoryDb(out var context);

            var updatedBook = new Book { Id = 1, Title = "Updated Title", AuthorId = 1 };

            var result = await controller.UpdateBook(1, updatedBook);
            Assert.IsType<NoContentResult>(result);

           
            var bookInDb = await context.Books
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == updatedBook.Id);

            Assert.NotNull(bookInDb);
            Assert.Equal(updatedBook.Id, bookInDb.Id);            
            Assert.Equal(updatedBook.Title, bookInDb.Title);       
            Assert.Equal(updatedBook.AuthorId, bookInDb.AuthorId); 
        }


        [Fact]
        public async Task DeleteBook_DeletesOnlyTargetedBook()
        {
            var controller = GetControllerWithInMemoryDb(out var context);

         
            int targetId = 2;
            var result = await controller.DeleteBook(targetId);

            Assert.IsType<NoContentResult>(result);

         
            var deletedBook = await context.Books.FindAsync(targetId);
            Assert.Null(deletedBook);  
        }



    }
}