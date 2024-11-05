namespace PriceSimulator.Domain.Entities.Bitstamp
{
    public class CurrencyPrice : BaseEntity
    {
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
    }
}
