using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PriceListener.Domain.Entities.Bitstamp
{
    public class BaseEntity
    {
        [JsonPropertyName("id")]
        [JsonProperty(PropertyName = "id")]
        [Key]
        public Guid Id { get; set; }

        public BaseEntity()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
