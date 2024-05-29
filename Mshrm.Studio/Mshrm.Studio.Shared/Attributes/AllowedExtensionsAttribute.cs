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
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="extensions"></param>
        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
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
            {
                return ValidationResult.Success;
            }

            // If the value exists, then check the file extension matches one in whitelist
            var file = value as IFormFile;
            if (!ValidateExtension(_extensions, file))
            {
                return new ValidationResult(GetErrorMessage());
            }

            // If we are here, then the extension is expected
            return ValidationResult.Success;
        }

        #region Helpers

        /// <summary>
        /// Gets an error message
        /// </summary>
        /// <returns></returns>
        private string GetErrorMessage()
        {
            return $"Your image's filetype is not valid.";
        }

        /// <summary>
        /// Validate extension
        /// </summary>
        /// <returns></returns>
        private bool ValidateExtension(string[] extensions, IFormFile file)
        {
            if (file != null)
            {
                // Get extension
                var extension = Path.GetExtension(file.FileName);

                // If it is not in out whitelist then error
                if (extensions.Contains(extension.ToLower()))
                    return true;
            }

            return false;
        }

        #endregion
    }
}
