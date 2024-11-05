using PriceListener.Domain.Entities.Bitstamp;
using System.Text.Json;

namespace PriceListener.Tests.Base.Converters
{
    public class UnixTimeMicrotimestampConvertToSecondsTest
    {

        [Fact]
        public void GivenValidJson_WhenConverted_ShouldReturnUnixTimeSeconds()
        {
            // arrange
            DateTimeOffset expected = new DateTimeOffset(638661759557584660, TimeSpan.Zero);

            // act
            OrderBook orderBook = JsonSerializer.Deserialize<OrderBook>(Constants.MSG_ORDER_BOOK_MICROTIMESTAMP);

            // assert
            Assert.Equal(expected, orderBook.Data.Microtimestamp);
        }
    }
}
