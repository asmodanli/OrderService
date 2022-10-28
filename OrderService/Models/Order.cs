using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace OrderService.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.Int32)]
        public int Id { get; set; }
        public int BrandId { get; set; }
        public decimal Price { get; set; }
        public string StoreName { get; set; }
        public string CustomerName { get; set; }
        public DateTime CreatedOn { get; set; }
        public OrderStatus Status { get; set; }
    }
}
