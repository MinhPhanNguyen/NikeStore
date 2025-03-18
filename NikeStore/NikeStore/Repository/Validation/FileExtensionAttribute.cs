using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace NikeStore.Repository.Validation
{
    public class FileExtensionAttribute : ValidationAttribute
    {
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName)?.ToLower(); // Chuẩn hóa chữ thường

                if (!_allowedExtensions.Contains(extension))
                {
                    return new ValidationResult($"Allowed file extensions are: {string.Join(", ", _allowedExtensions)}");
                }
            }
            return ValidationResult.Success;
        }
    }
}
