using OrderService.Composers.Interfaces;
using OrderService.Models;
using OrderService.Repositories.Interfaces;

namespace OrderService.Composers
{
    public class SearchOrderComposer : ISearchOrderComposer
    {
        private readonly ISearchOrderRepository _searchOrderRepository;
        public SearchOrderComposer(ISearchOrderRepository searchOrderRepository)
        {
            _searchOrderRepository = searchOrderRepository;

        }
        public async Task<List<Order>> SearchOrder(OrderFilter filter)
        {
            var orders = await _searchOrderRepository.GetOrder(filter);
            return orders;
        }
    }
}
