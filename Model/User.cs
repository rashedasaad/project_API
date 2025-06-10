namespace project_API.Model;

[Index(nameof(PhoneNumber), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
public class User
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "PhoneNumber is required")]
    [Phone(ErrorMessage = "Invalid PhoneNumber ")]
    
    public string PhoneNumber { get; set; }
    public bool Role { get; set; } = false; 
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
}