using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using project_API.Services.Payment.Services;

using System.Threading.Tasks;

namespace project_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IPaymentManager _paymentManager;
    private readonly IPaymentRepository _paymentRepo;
    private readonly IdataHelper<Order> _orderRepo;
    private readonly IOrderItemRepository _orderItemRepo;
    private readonly IProductRepository _productRepository;

    public PaymentController(IPaymentManager paymentManager, IPaymentRepository paymentRepo, IdataHelper<Order> orderRepo, IOrderItemRepository orderItemRepo, IProductRepository productRepository)
    {
        _paymentManager = paymentManager;
        _paymentRepo = paymentRepo;
        _orderRepo = orderRepo;
        _orderItemRepo = orderItemRepo;
        _productRepository = productRepository;
    }

    [HttpPost("save-payment")]
    public async Task<IActionResult> SavePayment([FromBody] SavePaymentDto paymentDto)
    {
        var payment = await _paymentRepo.FindByPredicate(
            p => p.Orderid == paymentDto.OrderId && p.Status.ToLower() == "initiated"
        );

        if (payment == null)
        {
            var exists = await _paymentRepo.FindByPredicate(p => p.Orderid == paymentDto.OrderId);
            if (exists == null)
                return BadRequest("Order not found");
            return BadRequest("Order found but status is not 'Initiated'");
        }

        // Get payment method from source
        string paymentMethod = "unknown";
        try
        {
            var sourceElement = (JsonElement)paymentDto.Payment.Source;
            if (sourceElement.ValueKind == JsonValueKind.Object && sourceElement.TryGetProperty("company", out var companyProp))
            {
                paymentMethod = companyProp.GetString() ?? "unknown";
            }
        }
        catch
        {
            paymentMethod = "unknown";
        }

        // Update order
        var order = await _orderRepo.FindByid( paymentDto.OrderId);
        if (order != null)
        {
            order.PaymentMethod = paymentMethod;
             _orderRepo.Edite(order);
        }

        // Update payment
        payment.PaymentId = paymentDto.Payment.Id;
        payment.Status = "Processing";
        await _paymentRepo.Edite(payment);

        return StatusCode(201, new { status = 201 });
    }

    [HttpGet("verify-payment")]
    public async Task<IActionResult> VerifyPayment([FromQuery] string id, [FromQuery] int orderId)
    {
        if (string.IsNullOrEmpty(id))
            return BadRequest("Payment ID is required");

        var result = await _paymentManager.VerifyPaymentAsync(id);
        if (result.IsSuccess)
        {
            var payment = await _paymentRepo.FindByPredicate(p => p.Orderid == orderId && p.PaymentId == id);
            {
                var orders = await _orderItemRepo.GetOrderItemsByOrderId(orderId);
                foreach (var order in orders)
                {
                    var product = await _productRepository.GetByIdAsync(order.ProductId);
                    product.Amount -= order.Quantity;
                    await _productRepository.UpdateAsync(product);
                }
                payment.Status = "Paid";
                await _paymentRepo.Edite(payment);
            }
        }

        return Ok(result);
    }
}

public class SavePaymentDto
{
    public PaymentData Payment { get; set; }
    public int OrderId { get; set; }

}

public class PaymentData
{
    public string Id { get; set; }
    public string Status { get; set; }
    public int Amount { get; set; }
    public string Created_At { get; set; }
    public string Currency { get; set; }
    public dynamic Source { get; set; }
    // Add more fields as needed from the JSON
}


