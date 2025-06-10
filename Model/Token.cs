
namespace project_API.Model;


public static class Time
{
    public static DateTime DateTimeNow
    {
        get
        {
            return DateTime.UtcNow;
        }
    }

    public static DateOnly DateOnlyNow
    {
        get
        {
            return DateOnly.FromDateTime(DateTimeNow);
        }
    }

    public static TimeOnly TimeOnlyNow
    {
        get
        {
            return TimeOnly.FromDateTime(DateTimeNow);
        }
    }
}

public class Token
{
    
  
    public int Id { get; set; }
    
    public required string Value { get;  set; } = string.Empty;
    public DateTime CreatedOn { get; set; } = Time.DateTimeNow;
    public DateTime ExpiresOn { get; set; }
    
    public int UserId { get; set; }
    public virtual User User { get; set; }

 


    public TimeSpan LeftTime
    {
        get
        {
            return ExpiresOn - CreatedOn;
        }
    }
}