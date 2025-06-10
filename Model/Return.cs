namespace project_API.Model;

public class Return
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public virtual Order Order { get; set; }
    public int ProductId { get; set; }
    public virtual Products Product { get; set; }
    public int Quantity { get; set; }
    public string Reason { get; set; }
    public string Status { get; set; }
    
}