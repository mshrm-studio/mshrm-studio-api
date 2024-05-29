using Mshrm.Studio.Shared.Models.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Subscription.Domain.PricingStructures.ValueObjects
{
    public class Discount : ValueObject
    {
        public decimal Percent { get; }
        public decimal Amount { get; }
        public string? Reason { get; }

        public Discount(decimal amount, decimal percent, string? reason)
        {
            if (percent < 0)
            {
                throw new ArgumentException(nameof(amount));
            }

            if (amount < 0)
            {
                throw new ArgumentException(nameof(amount));
            }

            Amount = amount;
            Percent = percent;
            Reason = reason;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            // Using a yield return statement to return each element one at a time
            yield return Percent;
            yield return Amount;
            yield return Reason;
        }
    }
}
