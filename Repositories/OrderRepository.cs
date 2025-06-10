namespace project_API.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly appdbcontext _context;

    public OrderRepository(appdbcontext context)
    {
        _context = context;
    }
 

    public async Task<IEnumerable<OrderDto>> GetAllOrders()
    {
        var orders = await _context.Payments
          //  .Where(p => p.Status == "Paid")
            .Include(p => p.Order)
            .ThenInclude(o => o.Order_item!)
            .ThenInclude(oi => oi.Product)
            .Include(p => p.Order!.User)
            .ToListAsync(); // Fetch data from database

        return orders
            .Where(p => p.Order != null)
            .Select(p => new OrderDto
            {
                Id = p.Order.Id,
                Name = p.Order.User.Name,
                Address = p.Order.ShippingAddress,
                PaymentMethod = p.Order.PaymentMethod,
                TotalAmount = p.Order.TotalAmount,
                Status = p.Order.Status,
                OrderDate = p.Order.OrderDate.HasValue 
                    ? p.Order.OrderDate.Value.ToString("yyyy-MM-dd HH:mm:ss")
                    : null,
                Items = p.Order.Order_item.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product.Name,
                    Price = oi.Price,
                    Quantity = oi.Quantity
                }).ToList()
            })
            .OrderByDescending(o => o.OrderDate) 
            .ToList();
    }


        


    public async Task<Order?> GetOrderById(int id)
    {
        return await _context.Orders.FindAsync(id);
    }

    public async Task<Order> CreateOrder(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
        
        return order;
    }

    public async Task<bool> UpdateOrder(Order order)
    {
        _context.Orders.Update(order);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteOrder(int id)
    {
        var order = await GetOrderById(id);
        if (order == null) return false;

        _context.Orders.Remove(order);
        return await _context.SaveChangesAsync() > 0;
    }
    public async Task<bool> UpdateOrderStatus(int orderId, string status)
    {
        var order = await GetOrderById(orderId);
        if (order == null) return false;

        order.Status = status;
        return await UpdateOrder(order);
    }
    
}