namespace project_API.view_models;

public class LoginRequest
{
    [Required(ErrorMessage = "PhoneNumber is required")]
    [Phone(ErrorMessage = "Invalid PhoneNumber ")]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Code is required")] 
    [StringLength(6, ErrorMessage = "Code must be 6 characters long")]
    public string Code { get; set; }
    
}
public class RegisterRequest
{
    
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }

    [Required(ErrorMessage = "PhoneNumber is required")]
    [Phone(ErrorMessage = "Invalid PhoneNumber ")]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; }
    
}

public class SendCodeRequest
{
    [Required(ErrorMessage = "PhoneNumber is required")]
    [Phone(ErrorMessage = "Invalid PhoneNumber ")]
    public string PhoneNumber { get; set; }
}
