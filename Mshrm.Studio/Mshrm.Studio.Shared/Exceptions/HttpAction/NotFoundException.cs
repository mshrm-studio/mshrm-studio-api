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
    /// To be thrown for Not Found Requests - 404
    /// </summary>
    public class NotFoundException : HttpActionValidationException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">A message for the exception</param>
        public NotFoundException(string failedReason, FailureCode failureCode, string? property = null) : base(HttpStatusCode.NotFound, failureCode, failedReason, property)
        { }
    }
}
