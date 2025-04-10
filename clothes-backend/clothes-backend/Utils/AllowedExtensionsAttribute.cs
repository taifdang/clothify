
using System.ComponentModel.DataAnnotations;

namespace clothes_backend.Utils
{
    public class AllowedExtensionsAttribute: ValidationAttribute
    {
        private readonly string[] _files;
        public AllowedExtensionsAttribute(string[] files)
        {
            _files = files;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var files = value as IFormFile[];
            //empty file
            if (files == null || files.Length == 0) return ValidationResult.Success!;
            //check file in files
            foreach(var item in files)
            {
                if(item.Length > 1024*1024) return new ValidationResult($"File {item.FileName} > 1MB");
                if (item == null || item.Length == 0) return ValidationResult.Success!;
                var extension = Path.GetExtension(item.FileName);
                if (!_files.Contains(extension.ToLower())) return new ValidationResult($"File extension of {item} is not allowed");
            }
            return ValidationResult.Success;
        }
    }
}
