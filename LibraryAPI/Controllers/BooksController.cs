﻿using LibraryAPI.Data;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly LibraryContext _context;

    public BooksController(LibraryContext context)
    {
        _context = context;
    }

    [HttpGet]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetBooks()
    {
        var books = await _context.Books
            .Select(b => new
            {
                b.Id,
                b.Title,
                b.AuthorId
               
            })
            .ToListAsync();

        return Ok(books);
    }


    [HttpPost]
    public async Task<ActionResult<Book>> CreateBook(Book book)
    {
        var authorExists = await _context.Authors.AnyAsync(a => a.Id == book.AuthorId);
        if (!authorExists)
        {
            return BadRequest("Author not found.");
        }

        //book.Title += "xxxxyz"; 
        //book.AuthorId = 6;
        //book.Id = 125;
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetBooks), new { id = book.Id }, book);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBook(int id, Book updatedBook)
    {
        if (id != updatedBook.Id)
            return BadRequest("Book ID mismatch.");

        var existingBook = await _context.Books.FindAsync(id);
        if (existingBook == null)
            return NotFound();


        existingBook.Title = updatedBook.Title;//+ "Not";
        existingBook.AuthorId = updatedBook.AuthorId; //+ 1;


        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var book = await _context.Books.FindAsync(id);//1545);
        if (book == null)
            return NotFound();

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();

        return NoContent();
    }

}