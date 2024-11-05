using Base;
using PriceListener.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PriceListener.Domain.Entities.Bitstamp
{
    [Table("OrderBooks")]
    public class OrderBook : OrderBookBase
    {
        [JsonPropertyName("data")]
        public OrderBookData Data { get; set; }
    }
}
