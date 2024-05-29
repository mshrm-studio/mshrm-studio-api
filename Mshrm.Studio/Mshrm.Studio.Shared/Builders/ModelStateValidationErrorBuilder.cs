using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Newtonsoft.Json;
using Mshrm.Studio.Shared.Extensions;
using Mshrm.Studio.Shared.Models;

namespace Mshrm.Studio.Shared.Builders
{
    public class ModelStateValidationErrorBuilder
    {
        /// <summary>
        /// Builds a bad request from the model state
        /// </summary>
        /// <param name="modelStateDictionary">The model state validation results</param>
        /// <returns>A bad request</returns>
        public static BadRequestObjectResult BuildBadRequest(ModelStateDictionary modelStateDictionary)
        {
            // Get errors
            var errors = modelStateDictionary.Keys
                .SelectMany(key => modelStateDictionary[key].Errors.Select(x => (key.LowercaseFirstLetter(), x.ErrorMessage)))
                .ToList();

            // Transform them to our format
            var transformedErrors = BuildErrors(errors.ToArray());

            // Build the error response
            var problemDetails = new MshrmStudioProblemDetails()
            {
                Detail = transformedErrors,
                StackTrace = null,
                Status = (int)HttpStatusCode.BadRequest,
                Title = "One or more validation errors occurred.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };

            // Return the resultant object
            return new BadRequestObjectResult(problemDetails);
        }

        #region Helpers

        /// <summary>
        /// Build errors (JObject) with a property-value
        /// </summary>
        /// <param name="propertyName">The property name</param>
        /// <param name="propertyValue">The property value</param>
        /// <returns>Error object</returns>
        private static string BuildErrors(params (string propertyName, string propertyValue)[] errors)
        {
            var errorObject = @"{}";

            var errorObjectToBuild = JsonConvert.DeserializeObject<Dictionary<string, object>>(errorObject);

            foreach (var error in errors)
            {
                // Check if it already exists - if so then ignore
                var alreadyExists = errorObjectToBuild.TryGetValue(error.propertyName, out _);
                if (!alreadyExists)
                    errorObjectToBuild.Add(error.propertyName, error.propertyValue);
            }

            return JsonConvert.SerializeObject(errorObjectToBuild);
        }

        #endregion
    }
}
