namespace project_API.Model;

public class Review
{
    public int Id { get; set; }
    
    
    [Required]
    [MaxLength(1000)]
    public string Comment { get; set; }
    
    [Range(1, 5)]
    public int Rating { get; set; }
    
    public int ProductId { get; set; }
    public virtual Products Product { get; set; }
    
    public int UserId { get; set; }
    public virtual User User { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
}