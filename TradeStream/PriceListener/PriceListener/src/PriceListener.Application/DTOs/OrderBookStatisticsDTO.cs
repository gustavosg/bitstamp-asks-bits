using PriceListener.Domain.Entities;
using System.Text;

namespace PriceListener.Application.DTOs
{
    public class OrderBookStatisticsDTO
    {
        public Cryptocurrency cryptocurrency { get;private set; }
        public decimal MaxAskPrice { get; private set; }
        public decimal MinAskPrice { get; private set; }
        public decimal MaxBidPrice { get; private set; }
        public decimal MinBidPrice { get; private set; }
        public decimal CurrentAverageAskPrice { get; private set; }
        public decimal CurrentAverageBidPrice { get; private set; }
        public decimal AverageAskPriceOverLast5Seconds { get; private set; }
        public decimal AverageBidPriceOverLast5Seconds { get; private set; }
        public decimal AverageAskQuantity { get; private set; }
        public decimal AverageBidQuantity { get; private set; }

        public OrderBookStatisticsDTO()
        {
            
        }

        public OrderBookStatisticsDTO(
            Cryptocurrency cryptocurrency,
            decimal maxAskPrice,
            decimal minAskPrice,
            decimal maxBidPrice,
            decimal minBidPrice,
            decimal currentAverageAskPrice,
            decimal currentAverageBidPrice,
            decimal averageAskPriceOverLast5Seconds,
            decimal averageBidPriceOverLast5Seconds,
            decimal averageAskQuantity,
            decimal averageBidQuantity
            )
        {
            this.cryptocurrency = cryptocurrency;
            this.MaxAskPrice = maxAskPrice;
            this.MinAskPrice = minAskPrice;
            this.MaxBidPrice = maxBidPrice;
            this.MinBidPrice = minBidPrice;
            this.CurrentAverageAskPrice = currentAverageAskPrice;
            this.CurrentAverageBidPrice = currentAverageBidPrice;
            this.AverageAskPriceOverLast5Seconds = averageAskPriceOverLast5Seconds;
            this.AverageBidPriceOverLast5Seconds = averageBidPriceOverLast5Seconds;
            this.AverageAskQuantity = averageAskQuantity;
            this.AverageBidQuantity = averageBidQuantity;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"Criptomoeda: {Enum.GetName(typeof(Cryptocurrency), this.cryptocurrency)}");
            builder.AppendLine($"Preço Máx. de Ask: {this.MaxAskPrice:N2}");
            builder.AppendLine($"Preço Min. de Ask: {this.MinAskPrice:N2}");
            builder.AppendLine($"Preço Máx. de Bid: {this.MaxBidPrice:N2}");
            builder.AppendLine($"Preço Min. de Bid: {this.MinBidPrice:N2}");
            builder.AppendLine($"Preço Médio Atual de Ask: {this.CurrentAverageAskPrice:N2}");
            builder.AppendLine($"Preço Médio Atual de Bid: {this.CurrentAverageBidPrice:N2}");
            builder.AppendLine($"Preço Médio Últimos 5 segs de Ask: {this.AverageAskPriceOverLast5Seconds:N2}");
            builder.AppendLine($"Preço Médio Últimos 5 segs de Bid: {this.AverageBidPriceOverLast5Seconds:N2}");
            builder.AppendLine($"Quantidade média de Ask: {this.AverageAskQuantity:N2}");
            builder.AppendLine($"Quantidade média de Bid: {this.AverageBidQuantity:N2}");
                          
            return builder.ToString();
        }
    }
}
