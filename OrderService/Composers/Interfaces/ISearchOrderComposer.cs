using OrderService.Models;

namespace OrderService.Composers.Interfaces
{
    public interface ISearchOrderComposer
    {
        Task<List<Order>> SearchOrder(OrderFilter filter);
    }
}
