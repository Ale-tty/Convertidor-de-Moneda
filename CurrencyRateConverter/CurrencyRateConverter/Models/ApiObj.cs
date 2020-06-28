
using System;
using System.Collections.Generic;
using System.Text;

namespace CurrencyRateConverter.Models
{
    public class ApiObj
    {
        public IDictionary<DateTime, ConversionRate> Rates { get; set; }
        public DateTime Start_at { get; set; }
        public DateTime End_at { get; set; }
        public string Base { get; set; }
    }
}
