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
    /// To be thrown for Forbidden Requests - 403
    /// </summary>
    public class ForbidException : HttpActionValidationException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">A message for the exception</param>
        public ForbidException(string failedReason, FailureCode failureCode, string? property = null) : base(HttpStatusCode.Forbidden, failureCode, failedReason, property)
        { }
    }
}
