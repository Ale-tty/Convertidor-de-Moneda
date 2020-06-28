using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CurrencyRateConverter.Models
{
    public class Rate
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public ConversionRate RatesData { get; set; }
    }
}
