namespace Shreco.API.Interfaces;
public interface IAuthService {
    Task<ObjectResult> Auth(string email);
    Task<ObjectResult> Register(User user);
}