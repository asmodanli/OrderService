using MongoDB.Bson;
using MongoDB.Driver;
using OrderService.Models;
using OrderService.Repositories.Interfaces;
using System;

namespace OrderService.Repositories
{
    public class SearchOrderRepository : ISearchOrderRepository
    {
        private readonly IMongoDatabase _orderDatabase;
        private readonly ILogger<InsertOrderRepository> _logger;
        private readonly IMongoClient _mongoClient;

        public SearchOrderRepository(ILogger<InsertOrderRepository> logger,
            IMongoClient mongoClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mongoClient = mongoClient ?? throw new ArgumentNullException(nameof(mongoClient));
            _orderDatabase = _mongoClient.GetDatabase("orderdb");
        }
        public async Task<List<Order>> GetOrder(OrderFilter filter)
        {

            var ordersCollection = _orderDatabase.GetCollection<Order>("orders");

            var builder = Builders<Order>.Filter;
            var storeNameFilter = builder.Eq("StoreName", filter.SearchText);
            var customerNameFilter = builder.Eq("CustomerName", filter.SearchText);
            var startDateFilter = builder.Gt("CreatedOn", filter.StartDate);
            var endDateFilter = builder.Lt("CreatedOn", filter.EndDate);
            var statusFilter = builder.In("Status", filter.Statuses);
            var skip = filter.PageNumber == 1 ? 0 : (filter.PageNumber - 1) * filter.PageSize + 1; 
            var orders = await ordersCollection.Find((storeNameFilter | customerNameFilter) 
                & startDateFilter
                & endDateFilter 
                & statusFilter)
                .Skip(skip)
                .Limit(filter.PageSize)
                .Sort(new BsonDocument(filter.SortBy, 1)).ToListAsync();

            return orders;
        }
    }
}
