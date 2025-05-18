using Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.User
{
    public interface IUserService
    {
        Task<string> CreateUserAsync(CreateUserRequestModel createUserRequestModel);
        Task<string> LoginAsync(string username, string password);
    }
}
