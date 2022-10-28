using Microsoft.Extensions.Logging;
using OrderService.Composers.Interfaces;
using OrderService.Models;
using OrderService.Repositories.Interfaces;

namespace OrderService.Composers
{
    public class InsertOrderComposer : IInsertOrderComposer
    {
        private readonly IInsertOrderRepository _insertOrderRepository;
        private readonly ILogger<InsertOrderComposer> _logger;

        public InsertOrderComposer(IInsertOrderRepository insertOrderRepository,
            ILogger<InsertOrderComposer> logger)
        {
            _insertOrderRepository = insertOrderRepository ?? throw new ArgumentNullException(nameof(insertOrderRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }
        public async Task<List<Order>?> InsertOrders(List<Order> orders)
        {
            var addedOrder = orders.Where(o => o.BrandId != 0).ToList();
            if (addedOrder.Any())
            {
               return await _insertOrderRepository.AddOrUpdateOrder(addedOrder);
            }
            else
            {
                return null;
            }
        }
    }
}
