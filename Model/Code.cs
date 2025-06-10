namespace project_API.Model;

public class Code
{
    public int Id { get; set; }
    public string CodeValue { get; set; }
    public DateTime ExpirationDate { get; set; }
    public bool IsUsed { get; set; }
    public int UserId { get; set; }
    public virtual User User { get; set; }
    public DateTime CreatedAt { get; set; }
    
   
   
}