using Data.DataContext;
using Microsoft.Extensions.Logging;
using Models.Book;
using System.Data.Entity;

namespace Business.Book
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<BookService> _logger;

        public BookService(ApplicationDbContext dbContext, ILogger<BookService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<string> CreateBookAsync(CreateBookRequestModel model)
        {
            try
            {
                var book = new Data.Entities.Book
                {
                    BookId = Guid.NewGuid().ToString(),
                    BookName = model.BookName,
                    Author = model.Author,
                    Price = model.Price,
                    PublishedDate = model.PublishedDate
                };

                _dbContext.Books.Add(book);
                await _dbContext.SaveChangesAsync();
                return $"Book '{book.BookName}' created successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating book");
                return $"Error creating book: {ex.Message}";
            }
        }

        public async Task<GetAllBookResponseModel> GetAllBookListAsync()
        {
            try
            {
                var books = _dbContext.Books.ToList();

                var response = new GetAllBookResponseModel
                {
                    Books = books.Select(b => new BookDto
                    {
                        BookId = b.BookId,
                        BookName = b.BookName,
                        Author = b.Author,
                        Price = b.Price,
                        PublishedDate = b.PublishedDate
                    }).ToList()
                };

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving book list");
                return new GetAllBookResponseModel
                {
                    Books = new List<BookDto>()
                };
            }
        }

        public async Task<string> UpdateBookAsync(UpdateBookRequestModel model)
        {
            try
            {
                var book = await _dbContext.Books.FindAsync(model.BookId);
                if (book == null)
                {
                    return $"Book with ID {model.BookId} not found.";
                }

                book.BookName = model.BookName;
                book.Author = model.Author;
                book.Price = model.Price;
                book.PublishedDate = model.PublishedDate;

                await _dbContext.SaveChangesAsync();
                return $"Book '{book.BookName}' updated successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating book");
                return $"Error updating book: {ex.Message}";
            }
        }

        public async Task<string> DeleteBook(string id)
        {
            try
            {
                var book = await _dbContext.Books.FindAsync(id);
                if (book == null)
                    return $"Book with ID {id} not found.";

                _dbContext.Books.Remove(book);
                await _dbContext.SaveChangesAsync();
                return $"Book '{book.BookName}' deleted successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting book");
                return $"Error deleting book: {ex.Message}";
            }
        }
    }

}

