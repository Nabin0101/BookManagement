using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SharedKernel.Validation
{
    public class UserNameValidationAttribute :ValidationAttribute
    {
        private readonly string _errorMessage;

        public UserNameValidationAttribute(string errorMessage = "Username must be 3-20 characters, letters and digits only.")
        {
            _errorMessage = errorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var username = value as string;
            if (string.IsNullOrEmpty(username))
            {
                return new ValidationResult(_errorMessage);
            }

            // Only letters and digits, length between 3 and 20
            var pattern = @"^[a-zA-Z0-9]{3,20}$";

            if (!Regex.IsMatch(username, pattern))
            {
                return new ValidationResult(_errorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
