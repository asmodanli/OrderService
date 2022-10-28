using OrderService.Models;
using MongoDB.Driver;
using OrderService.Repositories.Interfaces;

namespace OrderService.Repositories
{
    public class InsertOrderRepository : IInsertOrderRepository
    {
        private readonly IMongoDatabase _orderDatabase;
        private readonly ILogger<InsertOrderRepository> _logger;
        private readonly IMongoClient _mongoClient;
            
        public InsertOrderRepository(ILogger<InsertOrderRepository> logger,
            IMongoClient mongoClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mongoClient = mongoClient ?? throw new ArgumentNullException(nameof(mongoClient));
            _orderDatabase = _mongoClient.GetDatabase("orderdb");
        }

        public async Task<List<Order>?> AddOrUpdateOrder(List<Order> orders)
        {
            var ordersCollection = _orderDatabase.GetCollection<Order>("orders");
            var listWritesForOrders = new List<WriteModel<Order>>();

            orders.ForEach(order =>
            {
                var filterForOrders = Builders<Order>.Filter.Eq("_id", order.Id);
                listWritesForOrders.Add(new ReplaceOneModel<Order>(filterForOrders, order) { IsUpsert = true });
            });

            try
            {
                await ordersCollection.BulkWriteAsync(listWritesForOrders);
                _logger.LogInformation("[{now}] OrderService wrote the '{count}' documents of type 'orders' to the target.", DateTimeOffset.Now, orders.Count);
                return orders;
            }
            catch(MongoBulkWriteException bulkWriteException)
            {
                _logger.LogInformation("[{now}] OrderService was unable to write the document of type 'orders'. The exception message was {message}", DateTimeOffset.Now, bulkWriteException.Message);
                return null;
            }


        }
    }
}
