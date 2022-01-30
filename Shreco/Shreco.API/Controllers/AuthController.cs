namespace Shreco.API.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase {
    #region Fields
    private readonly IAuthService _authService;
    private readonly ICodeService _codeService;
    private readonly ITokenService _tokenService;
    #endregion
    public AuthController(IAuthService authService, ICodeService codeService, ITokenService tokenService) {
        _authService = authService;
        _codeService = codeService;
        _tokenService = tokenService;
    }

    [HttpPost("/{code}")]
    [Authorize(AuthenticationSchemes = "SessionJWT")]
    public async Task<IActionResult> Auth([FromBody] User user, string code) {
        var bearerToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        if (!await _codeService.CheckValidCode(_tokenService.GetNameIdentifer(bearerToken), code))
            return BadRequest("Code failed");
        if (ModelState.IsValid)
            return await _authService.Auth(user);
        return BadRequest("User not valid");
    }

    [HttpPost("/SendCode/{mail}")]
    public async Task<IActionResult> SendCode(string mail) {
        if (Regex.IsMatch(mail, @"^\S+@\S+\.\S+$"))
            return await _codeService.SendCode(mail);
        return BadRequest("This not mail");
    }

    [HttpPost("/QrAuth")]
    [Authorize(AuthenticationSchemes = "QrJWT")]
    public async Task<IActionResult> SendCode() {
        if (Regex.IsMatch(mail, @"^\S+@\S+\.\S+$"))
            return await _codeService.SendCode(mail);
        return BadRequest("This not mail");
    }
}