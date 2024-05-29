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
    /// To be thrown for Bad Requests - 400
    /// </summary>
    public class BadRequestException : HttpActionValidationException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">A message for the exception</param>
        public BadRequestException(string failedReason, FailureCode failureCode, string? property = null) : base(HttpStatusCode.Forbidden, failureCode, failedReason, property)
        { }
    }
}
