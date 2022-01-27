namespace Shreco.API.Services;
public class AuthService : ControllerBase, IAuthService {
    #region Fields
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    #endregion

    public AuthService(IUserService userService, ITokenService tokenService) {
        _userService = userService;
        _tokenService = tokenService;
    }

    public async Task<ObjectResult> Auth(User user) {
        try {
            if (await _userService.UserExist(user.Email))
                return Ok(_tokenService.CreateUserToken(user));
            await _userService.AddUser(user);
            await _userService.SaveChanges();
            return Ok(_tokenService.CreateUserToken(user));
        } catch {
            return BadRequest("Error add user");
        }
    }
}