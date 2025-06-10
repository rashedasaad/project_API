namespace project_API.Model;

public class Payment
{
    public int Id { get; set; }
    public int Orderid { get; set; }
    public virtual Order? Order { get; set; }
    public string PaymentId { get; set; } 
    public string Status { get; set; } 
    public decimal Amount { get; set; } 
    public string Currency { get; set; }
    public DateTime CreatedAt { get; set; }

}