﻿using System.Text.Json.Serialization;

namespace PriceListener.Domain.Entities.Bitstamp
{
    public class OrderBookBase : BaseEntity
    {
        [JsonPropertyName("channel")]
        public string Channel { get; set; }
        [JsonPropertyName("event")]
        public string Event { get; set; }
    }
}