namespace Shreco.API.Interfaces;
public interface IAuthService {
    Task<ObjectResult> Auth(User user);
}