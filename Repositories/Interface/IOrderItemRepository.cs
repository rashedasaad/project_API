namespace project_API.Repositories.Interface;

public interface IOrderItemRepository
{
    // Define methods for order item repository
    Task<IEnumerable<Order_item?>> GetAllOrderItems();
    Task<IEnumerable<Order_item?>> GetOrderItemsByOrderId(int id);

    
}