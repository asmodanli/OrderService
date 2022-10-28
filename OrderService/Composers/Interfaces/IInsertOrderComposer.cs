using OrderService.Models;

namespace OrderService.Composers.Interfaces
{
    public interface IInsertOrderComposer
    {
        Task<List<Order>?> InsertOrders(List<Order> orders);
    }
}
