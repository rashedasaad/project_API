namespace project_API.Services.Payment.Services;

public interface IPaymentManager
{
    Task<PaymentInitiationResult> InitiatePaymentAsync(int orderId, decimal amount, string currency, string description, string callbackUrl, string? Methods = "creditcard");

    Task<PaymentVerificationResult> VerifyPaymentAsync(string paymentId);

}