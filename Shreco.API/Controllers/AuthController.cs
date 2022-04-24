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

    [HttpGet]
    [Authorize(AuthenticationSchemes = "SessionJWT")]
    public async Task<IActionResult> Auth([FromQuery] string email, [FromQuery] string code) {
        var bearerToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        if (!await _codeService.CheckValidCode(TokenHelper.GetNameIdentifer(bearerToken), code))
            return BadRequest("Неправильный код!");
        return await _authService.Auth(email);
    }

    [HttpPost("Register/{code}")]
    [Authorize(AuthenticationSchemes = "SessionJWT")]
    public async Task<IActionResult> Register([FromBody] User user, [FromRoute]string code) {
        var bearerToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        if (!await _codeService.CheckValidCode(TokenHelper.GetNameIdentifer(bearerToken), code))
            return BadRequest("Неправильный код!");
        if (ModelState.IsValid)
            return await _authService.Register(user);
        return BadRequest("User not valid");
    }

    [HttpGet("SendCode/{email}")]
    public async Task<IActionResult> SendCode(string email) {
        if (Regex.IsMatch(email, @"^\S+@\S+\.\S+$"))
            return await _codeService.SendCode(email);
        return BadRequest("This not email");
    }

    //[HttpPost("/QrAuth")]
    //[Authorize(AuthenticationSchemes = "QrJWT")]
    //public async Task<IActionResult> SendCode() {
    //    if (Regex.IsMatch(mail, @"^\S+@\S+\.\S+$"))
    //        return await _codeService.SendCode(mail);
    //    return BadRequest("This not mail");
    //}
}