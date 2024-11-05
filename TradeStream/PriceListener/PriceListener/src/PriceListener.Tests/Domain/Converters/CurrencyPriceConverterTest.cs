using FluentAssertions;
using PriceListener.Domain.Entities.Bitstamp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PriceListener.Tests.Domain.Converters
{
    public class CurrencyPriceConverterTest
    {
        [Fact]
        public void GivenValidBidsJson_WhenConverted_ShouldReturnValidCurrencyPrice()
        {
            // arrange
            DateTimeOffset expected = new DateTimeOffset(638661759557584660, TimeSpan.Zero);
            List<CurrencyPrice> prices = new()
            {
                new () { Price = 69541, Amount = (decimal)0.26165453 },
                new () { Price = 69539, Amount = (decimal)0.14891605 },
            };

            // act
            OrderBook orderBook = JsonSerializer.Deserialize<OrderBook>(Constants.MSG_BODY_DATA_BIDS_LIST);

            // assert
            for (int i = 0; i < prices.Count; i++)
                prices[i].Should().BeEquivalentTo(orderBook.Data.Bids[i], options =>
                options.Excluding(bid => bid.Id));
        }
    }
}
