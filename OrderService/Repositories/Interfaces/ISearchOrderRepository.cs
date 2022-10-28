using OrderService.Models;

namespace OrderService.Repositories.Interfaces
{
    public interface ISearchOrderRepository
    {
        Task<List<Order>> GetOrder(OrderFilter filter);
    }
}
