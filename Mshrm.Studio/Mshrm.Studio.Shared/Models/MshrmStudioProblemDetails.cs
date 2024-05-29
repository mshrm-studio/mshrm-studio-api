using Microsoft.AspNetCore.Mvc;
using Mshrm.Studio.Shared.Enums;

namespace Mshrm.Studio.Shared.Models
{
    public class MshrmStudioProblemDetails : ProblemDetails
    {
        public string StackTrace { get; set; }
        public FailureCode FailureCode { get; set; }
    }
}
