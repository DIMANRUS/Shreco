namespace Shreco.API.Services;
public class AuthService : ControllerBase, IAuthService {
    #region Fields
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    #endregion

    public AuthService(IUserService userService, ITokenService tokenService)
    {
        _userService = userService;
        _tokenService = tokenService;
    }

    public async Task<ObjectResult> Auth(string email)
    {
        try {
            if (await _userService.GetUserByEmail(email) != null)
                return Ok(_tokenService.CreateToken(await _userService.GetUserByEmail(email)));
            return Unauthorized("Вас нет в системе, зарегистрируйтесь!");
        } catch {
            return BadRequest("Ошибка авторизации!");
        }
    }

    public async Task<ObjectResult> Register(User user)
    {
        //try {
        if (await _userService.GetUserByEmail(user.Email) != null)
            return Unauthorized("Почта уже зарегистрирована, войдите");
        await _userService.AddUsers(user);
        return Ok(_tokenService.CreateToken(user));
        //} catch {
        //    return BadRequest("Ошибка регистрации!");
        //}
    }
}