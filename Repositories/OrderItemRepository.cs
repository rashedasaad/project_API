namespace project_API.Repositories;

public class OrderItemRepository : IOrderItemRepository
{
    private readonly appdbcontext _db;

    public OrderItemRepository(appdbcontext db)
    {
        _db = db;
    
    }

    public async Task<IEnumerable<Order_item?>> GetAllOrderItems()
    {
        return await _db.Order_Items.ToListAsync();
     
    }

    public async Task<IEnumerable<Order_item?>> GetOrderItemsByOrderId(int orderId)
    {
        return await _db.Order_Items.Where(x => x.Orderid == orderId).ToListAsync();;
    }
    
}