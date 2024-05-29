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
    /// Base Http validation exception
    /// </summary>
    public abstract class HttpActionValidationException : Exception
    {
        /// <summary>
        /// The status code of the action
        /// </summary>
        public HttpStatusCode StatusCode { get; private set; }

        /// <summary>
        /// The validation error code
        /// </summary>
        public FailureCode FailureCode { get; set; }

        /// <summary>
        /// The validation error
        /// </summary>
        public string? FailedReason { get; set; }

        /// <summary>
        /// A model property tied to the error (nullable since not every execption can be tied to an input property)
        /// </summary>
        public string? Property { get; set; }

        /// <summary>
        /// A list of possible error reasons
        /// </summary>
        public List<(int index, string failedReason)> FailedReasons { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="statusCode">The http status code to associate with error</param>
        /// <param name="failureCode">The failure code</param>
        /// <param name="failedReason">The reason for the error being thrown</param>
        /// <param name="property">The property causing the error to be thrown (if any)</param>
        public HttpActionValidationException(HttpStatusCode statusCode, FailureCode failureCode, string failedReason, string? property = null) : base()
        {
            FailureCode = failureCode;
            StatusCode = statusCode;
            FailedReason = failedReason;
            Property = property;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="statusCode">The http status code to associate with error</param>
        /// <param name="reasons">The reason/s for the error being thrown</param>
        /// <param name="property">The property causing the error to be thrown (if any)</param>
        public HttpActionValidationException(string parentFailedReason, HttpStatusCode statusCode, List<(int index, string failedReason)> reasons, string? property = null) : base()
        {
            StatusCode = statusCode;
            Property = property;
            FailedReasons = reasons;
            FailedReason = parentFailedReason;
        }
    }
}
