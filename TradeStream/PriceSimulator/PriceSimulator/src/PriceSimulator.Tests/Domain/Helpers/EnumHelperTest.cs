﻿using PriceSimulator.Domain.Entities;
using PriceSimulator.Domain.Helpers;

namespace PriceSimulator.Tests.Domain.Helpers
{
    public class EnumHelperTest
    {
        [Theory]
        [InlineData(Cryptocurrency.BTC, "order_book_btcusd")]
        [InlineData(Cryptocurrency.ETH, "order_book_ethusd")]
        [InlineData((Cryptocurrency)999, "")]
        public void ValidateEnumHelper(Cryptocurrency cryptocurrency, string expected)
        {
            if (string.IsNullOrWhiteSpace(expected))
                Assert.Throws<NotImplementedException>(() =>
                    cryptocurrency.CryptocurrencyToSubscribeChannel()
                );
            else
                Assert.Equal(cryptocurrency.CryptocurrencyToSubscribeChannel(), expected);
        }
    }
}
