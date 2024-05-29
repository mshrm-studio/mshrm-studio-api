using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Shared.Attributes
{
    /// <summary>
    /// To ensure files have the correct type/extension
    /// </summary>
    public class MinFileSizeAttribute : ValidationAttribute
    {
        private readonly List<string> _extensions;
        private readonly long _minSize;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="extensions"></param>
        /// <param name="minSize"></param>
        public MinFileSizeAttribute(string[] extensions, long minSize)
        {
            _extensions = extensions.ToList();
            _minSize = minSize;
        }

        /// <summary>
        /// Validates a form files extension matches what has been set in whitelist upon instantiation
        /// </summary>
        /// <param name="value">The form file</param>
        /// <param name="validationContext">The context (request)</param>
        /// <returns>Our validation result</returns>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Check if the file provided is null - this is allowed and would measure success
            if (value == null)
                return ValidationResult.Success;

            // If the value exists, then check the file extension matches one in whitelist
            var file = value as IFormFile;
            if (file != null)
            {
                foreach (var extension in _extensions)
                {
                    if ((file.FileName?.ToLower()?.EndsWith(extension) ?? false) && file.Length < _minSize)
                        return new ValidationResult($"File too small as the minimum upload size is: {_minSize} bytes, try a bigger size");
                }
            }

            // If we are here, then the extension is expected
            return ValidationResult.Success;
        }
    }
}
