using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using project_API.Services.Payment.Services;
using System.Threading.Tasks;
using DotNetEnv;
using Microsoft.AspNetCore.Authorization;
using project_API.Services.Auth;

namespace project_API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IdataHelper<Order> _orderRepoDataHelper;
    private readonly IOrderRepository _orderRepo;
    private readonly IdataHelper<Order_item> _orderItemRepo;
    private readonly IdataHelper<Products> _productRepo;
    private readonly IPaymentManager _paymentManager;
    private readonly ClaimsReader _claimsReader;

    public OrdersController(
        IOrderRepository orderRepo,
        IdataHelper<Order_item> orderItemRepo,
        IdataHelper<Products> productRepo,
        IPaymentManager paymentManager, ClaimsReader claimsReader, IdataHelper<Order> orderRepoDataHelper)
    {
        _orderRepo = orderRepo;
        _orderItemRepo = orderItemRepo;
        _productRepo = productRepo;
        _paymentManager = paymentManager;
        _claimsReader = claimsReader;
        _orderRepoDataHelper = orderRepoDataHelper;
    }

    [HttpGet("ViewData")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await _orderRepo.GetAllOrders();
        if (orders == null || !orders.Any())
            return NotFound();

        return Ok(orders);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDto)
    {
        var userId = int.Parse(_claimsReader.GetByClaimType(ClaimTypes.NameIdentifier));

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Validate phonenumber  with whatsapp
        /*  var checkNumber = await _whatsAppManger.IsWhatsappNumber(request.PhoneNumber);

         if (checkNumber == false)
             return BadRequest("Invalid phone number.");

         */
        // Create the order
        var order = new Order
        {
            OrderDate = DateTime.UtcNow,
            Status = "قيد المعالجة",
            PaymentMethod = "0",
            ShippingAddress = orderDto.ShippingAddress,
            UserId = userId,
            TotalAmount = (double)orderDto.Total 
        };

         _orderRepoDataHelper.Add(order);

        // Add order items
        foreach (var item in orderDto.CartItems)
        {
            var product = await _productRepo.FindByid(item.ProductId);
            if (product == null)
                return BadRequest($"Product with ID {item.ProductId} not found");

            var orderItem = new Order_item
            {
                Orderid = order.Id,
                ProductId = item.ProductId,
                Price = product.Price,
                Quantity = item.Quantity
            };

             _orderItemRepo.Add(orderItem);
        }

        // Update order total
        await _orderItemRepo.UpdateOrderTotalAmount(order.Id);
        var callbackUrlTemplate = Env.GetString("Moyasar__CallbackUrl"); // Get the URL template
        var callbackUrl = callbackUrlTemplate.Replace("{orderId}", order.Id.ToString());
        // Initiate payment
        var paymentResult = await _paymentManager.InitiatePaymentAsync(
            orderId: order.Id, // Added missing orderId parameter
            amount: (decimal)order.TotalAmount,
            currency: "SAR",
            description: $"Order #{order.Id}",
            callbackUrl:   callbackUrl,
            Methods: order.PaymentMethod
        );

        if (!paymentResult.IsSuccess)
        {
            return BadRequest(paymentResult.Message);
        }

        return Ok(new { OrderId = order.Id, PaymentConfig = paymentResult.PaymentConfig });
    }

   

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> UpdateOrder(int id, [FromBody] CrudOrderDto orderDto)
    {
        var userId = int.Parse(_claimsReader.GetByClaimType(ClaimTypes.NameIdentifier));

        var order = await _orderRepoDataHelper.FindByid(id);
        if (order == null)
            return NotFound();

        order.OrderDate = orderDto.OrderDate;
        order.Status = orderDto.Stauts;
        order.PaymentMethod = orderDto.paymentMethod;
        order.ShippingAddress = orderDto.ShippingAddress;
        order.UserId = userId;

        _orderRepoDataHelper.Edite(order);
        return Ok(order);
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var order = await _orderRepoDataHelper.FindByid(id);
        if (order == null)
            return NotFound();

        _orderRepoDataHelper.Remove(order);
        return NoContent();
    }
    
    
    [HttpPost("UpdateStatus/{id}")]
    public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateStatus data)
    {
        var order = await _orderRepo.UpdateOrderStatus(id,data.Status);
        if (!order)
            return Ok(false);
        //Send email to user
        //Send Whatsapp to user
        return Ok(order);
    }
}

