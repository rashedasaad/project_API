using System.Runtime.InteropServices.JavaScript;
using System.Text.Json.Serialization;

namespace project_API.view_models
{
    
    public class CreateOrderDto
    {
        public string ShippingAddress { get; set; }
        public List<CartItemDto> CartItems { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Shipping { get; set; }
        public decimal Total { get; set; }
    }

    public class CartItemDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

    public class CrudOrderDto
    {
        public DateTime OrderDate { get; set; }
        public string Stauts { get; set; } // Note: Typo in original code, should be Status
        public string paymentMethod { get; set; }
        public string ShippingAddress { get; set; }
    }
    
    public class OrderDto
    {
        
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<OrderItemDto>? Items { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Address { get; set; }
        public double TotalAmount { get; set; }
        public string? Status { get; set; }
        public string? OrderDate { get; set; }
    }


    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public double Price { get; set; }
        public double Quantity { get; set; }
    }
    public class UpdateStatus
    {
          public string Status { get; set; }
    }
    
}
    
