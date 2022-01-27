namespace Shreco.API.Interfaces;

public interface ITokenService {
    string CreateUserToken(User user);
    string CreateSessionToken(Session session);
    string GetNameIdentifer(string token);
    string GetName(string token);
}