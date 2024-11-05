using PriceSimulator.Domain.Entities.Bitstamp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceSimulator.Domain.Entities.TradeStream
{
    [Table("Prices")]
    public class Price : CurrencyPrice
    {
    }
}
