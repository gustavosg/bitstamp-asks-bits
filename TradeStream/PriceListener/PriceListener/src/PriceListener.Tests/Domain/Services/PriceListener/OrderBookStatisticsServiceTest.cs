using PriceListener.Domain.Entities;
using PriceListener.Domain.Entities.Bitstamp;
using PriceListener.Domain.Helpers;
using PriceListener.Domain.Services.PriceListener;
using System.Text.Json;

namespace PriceListener.Tests.Domain.Services.PriceListener
{
    public class OrderBookStatisticsServiceTest
    {
        private readonly OrderBookStatisticsService service = new();

        private readonly List<OrderBook> orderBookList = JsonSerializer.Deserialize<List<OrderBook>>(Constants.MSG_LIST_RECEIVED_BTC);

        [Fact]
        public void GivenOrderBooks_WhenStatisticsExecuted_ShouldReturnAverageAsksLastFiveSeconds()
        {
            this.orderBookList.ForEach(item =>
            {
                item.Data.Timestamp = DateTime.UtcNow;
                item.Data.Microtimestamp = DateTime.UtcNow;
            });
            decimal expected = 68396.27m;

            // act
            decimal actual = this.service.GetAveragePriceAsksOverLastFiveSeconds(this.orderBookList);

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GivenOrderBooks_WhenStatisticsExecuted_ShouldReturnAverageBidsLastFiveSeconds()
        {
            // arrange

            this.orderBookList.ForEach(item =>
            {
                item.Data.Timestamp = DateTime.UtcNow;
                item.Data.Microtimestamp = DateTime.UtcNow;
            });
            decimal expected = 67914.6m;

            // act
            decimal actual = this.service.GetAveragePriceBidsOverLastFiveSeconds(this.orderBookList);

            // assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(Cryptocurrency.BTC, 68106d)]
        [InlineData(Cryptocurrency.ETH, 28106d)]
        public void GivenOrderBooks_WhenStatisticsExecuted_ShouldReturnAskMinPrice(Cryptocurrency cryptocurrency, decimal expected)
        {
            // arrange
            OrderBookData orderBookData = this.orderBookList
                .Where(x => string.Equals(x.Channel, cryptocurrency.CryptocurrencyToSubscribeChannel()))
                .OrderByDescending(x => x.Data.Timestamp)
                .FirstOrDefault()?.Data
                ?? new();

            // act
            decimal actual = this.service.GetMinPrice(orderBookData.Asks.ToList<CurrencyPrice>());

            // assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(Cryptocurrency.BTC, 68614d)]
        [InlineData(Cryptocurrency.ETH, 38111d)]
        public void GivenOrderBooks_WhenStatisticsExecuted_ShouldReturnAskMaxPrice(Cryptocurrency cryptocurrency, decimal expected)
        {
            // arrange
            OrderBookData orderBookData = this.orderBookList
                .Where(x => string.Equals(x.Channel, cryptocurrency.CryptocurrencyToSubscribeChannel()))
                .OrderByDescending(x => x.Data.Timestamp)
                .FirstOrDefault()?.Data
                ?? new();

            // act
            decimal actual = this.service.GetMaxPrice(orderBookData.Asks.ToList<CurrencyPrice>());

            // assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(Cryptocurrency.BTC, 67650d)]
        [InlineData(Cryptocurrency.ETH, 11104d)]
        public void GivenOrderBooks_WhenStatisticsExecuted_ShouldReturnBidMinPrice(Cryptocurrency cryptocurrency, decimal expected)
        {
            // arrange
            OrderBookData orderBookData = this.orderBookList
                .Where(x => string.Equals(x.Channel, cryptocurrency.CryptocurrencyToSubscribeChannel()))
                .OrderByDescending(x => x.Data.Timestamp)
                .FirstOrDefault()?.Data
                ?? new();

            // act
            decimal actual = this.service.GetMinPrice(orderBookData.Bids.ToList<CurrencyPrice>());

            // assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(Cryptocurrency.BTC, 68105d)]
        [InlineData(Cryptocurrency.ETH, 78105d)]
        public void GivenOrderBooks_WhenStatisticsExecuted_ShouldReturnBidMaxPrice(Cryptocurrency cryptocurrency, decimal expected)
        {
            // arrange
            OrderBookData orderBookData = this.orderBookList
                .Where(x => string.Equals(x.Channel, cryptocurrency.CryptocurrencyToSubscribeChannel()))
                .OrderByDescending(x => x.Data.Timestamp)
                .FirstOrDefault()?.Data
                ?? new();

            // act
            decimal actual = this.service.GetMaxPrice(orderBookData.Bids.ToList<CurrencyPrice>());

            // assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(Cryptocurrency.BTC, 1.1492147358)]
        [InlineData(Cryptocurrency.ETH, 2.76490000)]
        public void GivenOrderBooks_WhenStatisticsExecuted_ShouldReturnAvgAskQuantity(Cryptocurrency crypto, decimal expected)
        {
            // arrange
            OrderBookData orderBookData = this.orderBookList
                .Where(x => string.Equals(x.Channel, crypto.CryptocurrencyToSubscribeChannel()))
                .OrderByDescending(x => x.Data.Timestamp)
                .FirstOrDefault()?.Data ?? new();

            // act

            decimal actual = this.service.GetAverageQuantity(orderBookData.Asks.ToList<CurrencyPrice>());

            // assert

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(Cryptocurrency.BTC, 0.8358042377)]
        [InlineData(Cryptocurrency.ETH, 0.5836572766666667)]
        public void GivenOrderBooks_WhenStatisticsExecuted_ShouldReturnAvgBidQuantity(Cryptocurrency crypto, decimal expected)
        {
            // arrange
            OrderBookData orderBookData = this.orderBookList
                .Where(x => string.Equals(x.Channel, crypto.CryptocurrencyToSubscribeChannel()))
                .OrderByDescending(x => x.Data.Timestamp)
                .FirstOrDefault()?.Data ?? new();

            // act
            decimal actual = this.service.GetAverageQuantity(orderBookData.Bids.ToList<CurrencyPrice>());

            // assert
            Assert.Equal(expected, actual, 15);
        }

        [Theory]
        [InlineData(Cryptocurrency.BTC, 68410.74)]
        [InlineData(Cryptocurrency.ETH, 33108.5)]
        public void GivenOrderBookData_WhenStatisticsExecuted_ShouldReturnAverageAskPrice(Cryptocurrency crypto, decimal expected)
        {
            // arrange
            OrderBookData orderBookData = this.orderBookList
                .Where(x => string.Equals(x.Channel, crypto.CryptocurrencyToSubscribeChannel()))
                .OrderByDescending(x => x.Data.Timestamp)
                .FirstOrDefault()?.Data ?? new();

            // act
            decimal actual = this.service.GetAveragePrice(orderBookData.Asks.ToList<CurrencyPrice>());

            // assert
            Assert.Equal(expected, actual, 15);
        }
        
        [Theory]
        [InlineData(Cryptocurrency.BTC, 67910.16)]
        [InlineData(Cryptocurrency.ETH, 52437)]
        public void GivenOrderBookData_WhenStatisticsExecuted_ShouldReturnAverageBidPrice(Cryptocurrency crypto, decimal expected)
        {
            // arrange
            OrderBookData orderBookData = this.orderBookList
                .Where(x => string.Equals(x.Channel, crypto.CryptocurrencyToSubscribeChannel()))
                .OrderByDescending(x => x.Data.Timestamp)
                .FirstOrDefault()?.Data ?? new();

            // act
            decimal actual = this.service.GetAveragePrice(orderBookData.Bids.ToList<CurrencyPrice>());

            // assert
            Assert.Equal(expected, actual, 15);
        }
    }
}
