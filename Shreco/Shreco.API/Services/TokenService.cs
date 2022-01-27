namespace Shreco.API.Services;

public class TokenService : ITokenService {
    private readonly IConfiguration _configuration;
    public TokenService(IConfiguration configuration) =>
        _configuration = configuration;
    #region Private Methods
    private static IEnumerable<Claim> GetClaims(string token) =>
        new JwtSecurityTokenHandler().ReadJwtToken(token).Claims;
    #endregion
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
    public string GetNameIdentifer(string token) =>
        GetClaims(token).First(c => c.Type == ClaimTypes.NameIdentifier).Value;
    public string GetName(string token) =>
        GetClaims(token).First(c => c.Type == ClaimTypes.Name).Value;
    #endregion
}