using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SharedKernel.Validation
{
    public class PasswordValidationAttribute : ValidationAttribute
    {
        private readonly string _errorMessage;

        public PasswordValidationAttribute(string errorMessage = "Password must be at least 8 characters, contain uppercase, lowercase, digit and special character.")
        {
            _errorMessage = errorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var password = value as string;
            if (string.IsNullOrEmpty(password))
            {
                return new ValidationResult(_errorMessage);
            }

            // Minimum 8 chars, at least one uppercase, one lowercase, one digit and one special char
            var pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$";

            if (!Regex.IsMatch(password, pattern))
            {
                return new ValidationResult(_errorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
