namespace Shreco.API.Services;

public class TokenService : ITokenService {
    private readonly IConfiguration _configuration;
    public TokenService(IConfiguration configuration) =>
        _configuration = configuration;
    #region Public Methods
    public string CreateUserToken(User user) {
        var claims = new[] {
            new Claim(ClaimTypes.Name, user.NameIdentifer),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.StreetAddress, user.Adress ?? string.Empty)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
        var token = new JwtSecurityToken(
            _configuration["JwtSettings:Issuer"],
            _configuration["JwtSettings:Audience"],
            claims: claims,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public string CreateSessionToken(Session session) {
        var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, session.SessionId)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SessionKey"]));
        var token = new JwtSecurityToken(
            _configuration["JwtSettings:Issuer"],
            _configuration["JwtSettings:Audience"],
            expires: DateTime.Now.AddMinutes(10),
            claims: claims,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public string CreateQrToken(Qr qr) {
        var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, qr.Id.ToString()),
            new Claim(ClaimTypes.Name, qr.WhoCreated.Id.ToString()),
            new Claim(ClaimTypes.Role, qr.QrType.ToString())
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:QrKey"]));
        var token = new JwtSecurityToken(
            _configuration["JwtSettings:Issuer"],
            _configuration["JwtSettings:Audience"],
            expires: DateTime.Now.AddMinutes(10),
            claims: claims,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    #endregion
}