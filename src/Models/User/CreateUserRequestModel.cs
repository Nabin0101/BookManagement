using SharedKernel.Validation;
using System.ComponentModel.DataAnnotations;

namespace Models.User
{
    public class CreateUserRequestModel
    {
        [Required]
        [UserNameValidationAttribute]
        public string Username { get; set; }

        [Required]
        [PasswordValidationAttribute]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string FullName { get; set; }
       
    }
}
