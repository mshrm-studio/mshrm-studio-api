using Mshrm.Studio.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Shared.Exceptions.HttpAction
{
    /// <summary>
    /// To be thrown for Unprocessable Entity Requests - 422
    /// </summary>
    public class UnprocessableEntityException : HttpActionValidationException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">A message for the exception</param>
        public UnprocessableEntityException(string failedReason, FailureCode failureCode, string? property = null) : base(HttpStatusCode.UnprocessableEntity, failureCode, failedReason, property)
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">A message for the exception</param>
        public UnprocessableEntityException(string parentFailedReason, List<(int index, string failedReason)> childFailedReasons, string? property = null) : base(parentFailedReason, HttpStatusCode.UnprocessableEntity, childFailedReasons, property)
        { }
    }
}
