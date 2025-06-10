namespace project_API.Services.Email.Interface;

public interface IEmailManger
{
    Task<bool> SendEmailAsync(string to, string subject, string body);
    Task<bool> SendInvoiceEmailHtmlAsync(string toEmail, string htmlContent);

}