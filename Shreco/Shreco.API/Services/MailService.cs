namespace Shreco.API.Services;

public class MailService : IMailService {
    private readonly IConfiguration _configuration;
    public MailService(IConfiguration configuration) =>
        _configuration = configuration;
    public async Task<bool> SendMailWithCode(string mail, string code) {
        using MailMessage mailMessage = new();
        using SmtpClient smtp = new("smtp.yandex.ru");
        mailMessage.From = new MailAddress("dimanrus@dimanrusdev.ru");
        mailMessage.To.Add(mail);
        mailMessage.Subject = "КОд подтверждения | Shreco";
        mailMessage.Body = $"Ваш код: {code}";
        mailMessage.IsBodyHtml = true;
        smtp.Port = 587;
        smtp.Credentials = new NetworkCredential("dimanrus@dimanrusdev.ru", _configuration.GetRequiredSection("MailSettings").GetSection("Password").Value);
        smtp.EnableSsl = true;
        try {
            await smtp.SendMailAsync(mailMessage);
            return true;
        } catch {
            return false;
        }
    }
}