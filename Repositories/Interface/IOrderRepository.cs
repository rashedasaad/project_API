namespace project_API.Repositories.Interface;

public interface IOrderRepository
{
    Task<IEnumerable<OrderDto>> GetAllOrders();
    Task<Order> GetOrderById(int id);
    Task<Order> CreateOrder(Order order);
    Task<bool> UpdateOrder(Order order);
    Task<bool> DeleteOrder(int id);
    Task<bool> UpdateOrderStatus(int orderId, string status);
 

    
}