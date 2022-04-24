namespace Shreco.API.Interfaces;

public interface ITokenService {
    string CreateToken(User user);
    string CreateToken(Session session);
    string CreateToken(Qr qr);
}