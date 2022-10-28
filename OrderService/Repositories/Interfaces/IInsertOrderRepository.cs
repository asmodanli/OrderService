using OrderService.Models;

namespace OrderService.Repositories.Interfaces
{
    public interface IInsertOrderRepository
    {
        Task<List<Order>?> AddOrUpdateOrder(List<Order> orders);
    }
}
