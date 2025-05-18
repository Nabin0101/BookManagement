using Business.Book;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Book;

namespace WebAPI.Controller.Book
{
    public class BookController : BaseApiController
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] CreateBookRequestModel model)
        {
            try
            {
                var result = await _bookService.CreateBookAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating book: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            try
            {
                var books = await _bookService.GetAllBookListAsync();
                return Ok(books);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching books: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBook([FromBody] UpdateBookRequestModel model)
        {
            try
            {
                var result = await _bookService.UpdateBookAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating book: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(string id)
        {
            try
            {
                var result = await _bookService.DeleteBook(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting book: {ex.Message}");
            }
        }

        
    }
}
