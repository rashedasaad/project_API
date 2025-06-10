using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using project_API.Services.Email.Interface;
using DotNetEnv;
namespace project_API.Services.Email;

public class EmailManger: IEmailManger
{

    private readonly string _smtpHost;
    private readonly string _smtpPort;
    private readonly string _smtpUsername;
    private readonly string _smtpPassword;

    public EmailManger()
    {
        _smtpHost = Env.GetString("SMTP_Host");;
        _smtpPort = Env.GetString("SMTP_Port");;
        _smtpUsername = Env.GetString("SMTP_Username");;
        _smtpPassword = Env.GetString("SMTP_Password");;
        
        
    }
    public Task<bool> SendEmailAsync(string to, string subject, string body)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> SendInvoiceEmailHtmlAsync(string toEmail, string htmlContent)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_smtpUsername));
        email.To.Add(MailboxAddress.Parse(toEmail));
        email.Subject = "Invoice";
        email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlContent };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_smtpHost, Int32.Parse(_smtpPort), SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_smtpUsername, _smtpPassword);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
        return true;
    }

    
}