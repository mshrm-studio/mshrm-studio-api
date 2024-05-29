using Microsoft.AspNetCore.Mvc;

namespace Mshrm.Studio.Subscription.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubscriptionController : ControllerBase
    {
        private readonly ILogger<SubscriptionController> _logger;

        public SubscriptionController(ILogger<SubscriptionController> logger)
        {
            _logger = logger; 
        }

    }
}
