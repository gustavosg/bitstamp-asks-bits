using PriceSimulator.Domain.Entities;
using PriceSimulator.Domain.Entities.Bitstamp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceSimulator.Application.DTOs
{
    public class BestPriceDTO
    {
        public Guid Id { get; set; }
        List<CurrencyPrice> Prices { get; set; }
        public float Amount { get; set; }
        public OperationType OperationType { get; set; }
        public decimal Value { get; set; }
    }
}
