
public class PaymentInitiationResult
{
    public bool IsSuccess { get; set; }
    public PaymentConfig PaymentConfig { get; set; }
    public string Message { get; set; }
}

public class PaymentConfig
{
    public int Amount { get; set; }
    public string Currency { get; set; }
    public string Description { get; set; }
    public string PublishableApiKey { get; set; }
    public string CallbackUrl { get; set; }
    public string[] Methods { get; set; }
    public Dictionary<string, string> Metadata { get; set; }

}

public class PaymentVerificationResult
{
    public bool IsSuccess { get; set; }
    public string PaymentId { get; set; }
    public string Status { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string Message { get; set; }
}

public class MoyasarPaymentResponse
{
    public string Id { get; set; }
    public string Status { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
}



