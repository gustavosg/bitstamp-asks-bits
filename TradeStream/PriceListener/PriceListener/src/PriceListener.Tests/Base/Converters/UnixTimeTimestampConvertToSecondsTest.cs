using PriceListener.Domain.Entities.Bitstamp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PriceListener.Tests.Base.Converters
{
    public class UnixTimeTimestampConvertToSecondsTest
    {

        [Fact]
        public void GivenValidJson_WhenConverted_ShouldReturnUnixTimeSeconds()
        {
            // arrange
            DateTime expected = new DateTime(2024, 11, 2, 17, 25, 55).ToUniversalTime();

            // act
            OrderBook orderBook = System.Text.Json.JsonSerializer.Deserialize<OrderBook>(Constants.MSG_ORDER_BOOK_TIMESTAMP);

            // assert
            Assert.Equal(expected, orderBook.Data.Timestamp);
        }
    }
}
