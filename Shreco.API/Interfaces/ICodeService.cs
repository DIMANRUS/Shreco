namespace Shreco.API.Interfaces;

public interface ICodeService {
    Task<ObjectResult> SendCode(string mail);
    Task<bool> CheckValidCode(string session, string userCode);
}