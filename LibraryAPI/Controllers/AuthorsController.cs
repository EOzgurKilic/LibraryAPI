using System.Reflection.Metadata.Ecma335;
using LibraryAPI.Data;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly LibraryContext _context;

    public AuthorsController(LibraryContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
    {
        return await _context.Authors.Include(a => a.Books).ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Author>> CreateAuthor(Author author)
    {
        /*author.Name = "CodeErrora";
        author.Id = 1638;*/
        _context.Authors.Add(author);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAuthors), new { id = author.Id }, author);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAuthor(int id, Author updatedAuthor)
    {
        if (id != updatedAuthor.Id)
        {
            return BadRequest("Author ID mismatch.");
        }

        var existingAuthor = await _context.Authors.FindAsync(id);//3216);
        if (existingAuthor == null)
        {
            return NotFound();
        }

        existingAuthor.Name = updatedAuthor.Name; //+ "asdsacx";
        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAuthor(int id)
    {
        var author = await _context.Authors.Include(a => a.Books).FirstOrDefaultAsync(a => a.Id == id); //6549);
        if (author == null)
        {
            return NotFound();
        }

      
        _context.Books.RemoveRange(author.Books);

        _context.Authors.Remove(author);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}