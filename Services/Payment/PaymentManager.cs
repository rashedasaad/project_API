// project_API/Services/Payment/Services/PaymentManager.cs

using System.Text;
using System.Text.Json;
using DotNetEnv;
using project_API.Services.Payment.Services;
using project_API.Model;

namespace project_API.Services.Payment;



public class PaymentManager : IPaymentManager
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IPaymentRepository _paymentRepo;
    private readonly string _moyasarSecretKey;
    private readonly string _moyasarPublishableKey;
    private readonly string _moyasarBaseUrl;

    public PaymentManager(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        IPaymentRepository paymentRepo)
    {
        _httpClientFactory = httpClientFactory;
        _paymentRepo = paymentRepo;
        _moyasarSecretKey = Env.GetString("Moyasar__SecretKey");
        _moyasarPublishableKey = Env.GetString("Moyasar__PublishableKey");
        _moyasarBaseUrl = Env.GetString("Moyasar__BaseUrl");
    }
    public async Task<PaymentInitiationResult> InitiatePaymentAsync(
        int orderId,
        decimal amount,
        string currency,
        string description,
        string callbackUrl,
        string? Methods = "creditcard")
    {
        try
        {
            if (amount <= 0)
                return new PaymentInitiationResult { IsSuccess = false, Message = "Amount must be greater than zero." };

            if (string.IsNullOrEmpty(currency) || currency.Length != 3)
                return new PaymentInitiationResult { IsSuccess = false, Message = "Invalid currency code." };

            if (string.IsNullOrEmpty(callbackUrl) || !Uri.IsWellFormedUriString(callbackUrl, UriKind.Absolute))
                return new PaymentInitiationResult { IsSuccess = false, Message = "Invalid callback URL." };

            var payment = new Model.Payment
            {
                Orderid = orderId,
                Status = "Initiated",
                Amount = amount,
                Currency = currency,
                CreatedAt = DateTime.UtcNow,
                PaymentId = Guid.NewGuid().ToString(),
                
                
            };

            await _paymentRepo.Add(payment); 

            var paymentConfig = new PaymentConfig
            {
                Amount = (int)(amount * 100),
                Currency = currency,
                Description = description,
                PublishableApiKey = _moyasarPublishableKey,
                CallbackUrl = callbackUrl,
                Methods = new[] {"creditcard","stcpay"}, 
                Metadata = new Dictionary<string, string>
                {
                    { "Store", Env.GetString("Store_Name")},
                    { "OrderId", orderId.ToString() }
                    
                }
                
            };

            return new PaymentInitiationResult { IsSuccess = true, PaymentConfig = paymentConfig };
        }
        catch (Exception ex)
        {
            return new PaymentInitiationResult { IsSuccess = false, Message = $"Error initiating payment: {ex.Message}" };
        }
    }

    public async Task<PaymentVerificationResult> VerifyPaymentAsync(string paymentId)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_moyasarSecretKey}:"))
            );

            var response = await httpClient.GetAsync($"{_moyasarBaseUrl}/payments/{paymentId}");

            if (!response.IsSuccessStatusCode)
            {
                return new PaymentVerificationResult
                {
                    IsSuccess = false,
                    PaymentId = paymentId,
                    Status = "failed",
                    Message = $"Failed to fetch payment: {response.StatusCode}"
                };
            }

            var content = await response.Content.ReadAsStringAsync();
            var payment = JsonSerializer.Deserialize<MoyasarPaymentResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (payment == null)
            {
                return new PaymentVerificationResult
                {
                    IsSuccess = false,
                    PaymentId = paymentId,
                    Status = "failed",
                    Message = "Invalid payment response"
                };
            }

            var dbPayment = await _paymentRepo.FindByPredicate(p => p.PaymentId == paymentId);
            if (dbPayment != null)
            {
                dbPayment.Status = payment.Status;
                await _paymentRepo.Edite(dbPayment);
            }

            return new PaymentVerificationResult
            {
                IsSuccess = payment.Status == "paid",
                PaymentId = payment.Id,
                Status = payment.Status,
                Amount = payment.Amount / 100m,
                Currency = payment.Currency,
                Message = payment.Status == "paid" ? "Payment succeeded" : "Payment failed"
            };
        }
        catch (Exception ex)
        {
            return new PaymentVerificationResult
            {
                IsSuccess = false,
                PaymentId = paymentId,
                Status = "failed",
                Message = $"Error verifying payment: {ex.Message}"
            };
        }
    }
}

