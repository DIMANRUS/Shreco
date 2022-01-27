namespace Shreco.API.Interfaces;

public interface IMailService {
    Task<bool> SendMailWithCode(string mail,string code);
}