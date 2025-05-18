using Models.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Book
{
    public  interface IBookService
    {
        Task<string> CreateBookAsync(CreateBookRequestModel createBookRequestModel);
        Task<GetAllBookResponseModel> GetAllBookListAsync();
        Task<string> UpdateBookAsync(UpdateBookRequestModel updateBookRequestModel);
        Task<string> DeleteBook(string id);
    }
}
