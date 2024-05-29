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
    /// To be thrown for Unauthorized Requests - 401
    /// </summary>
    public class UnauthorizedException : HttpActionValidationException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">A message for the exception</param>
        public UnauthorizedException(string failedReason, FailureCode failureCode, string? property = null) : base(HttpStatusCode.Forbidden, failureCode, failedReason, property)
        { }
    }
}
