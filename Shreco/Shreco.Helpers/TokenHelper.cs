using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Shreco.Helpers;

public class TokenHelper {
    private static IEnumerable<Claim> GetClaims(string token) =>
        new JwtSecurityTokenHandler().ReadJwtToken(token).Claims;
    public static string GetNameIdentifer(string token) =>
        GetClaims(token).First(c => c.Type == ClaimTypes.NameIdentifier).Value;
    public static string GetName(string token) =>
        GetClaims(token).First(c => c.Type == ClaimTypes.Name).Value;
    public static string GetRole(string token) =>
        GetClaims(token).First(c => c.Type == ClaimTypes.Role).Value;
    public static string GetStreetAddress(string token) =>
        GetClaims(token).First(c => c.Type == ClaimTypes.StreetAddress).Value;
    public static string GetEmail(string token) =>
        GetClaims(token).First(c => c.Type == ClaimTypes.Email).Value;
    public static string GetActor(string token) =>
        GetClaims(token).First(c => c.Type == ClaimTypes.Actor).Value;
    public static string GetMobilePhone(string token) =>
        GetClaims(token).First(c => c.Type == ClaimTypes.MobilePhone).Value;
}