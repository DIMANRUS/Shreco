namespace Shreco.API.Controllers;
[Route("[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    public UserController(IUserService userService, ITokenService tokenService)
    {
        _userService = userService;
        _tokenService = tokenService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUser()
    {
        var bearerToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        User? user = await _userService.GetUserByEmail(TokenHelper.GetEmail(bearerToken));
        if (user != null)
            return Ok(user);
        return BadRequest("Пользователь не найден");
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> UpdateUser(User user)
    {
        var bearerToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        User? userFromDb = await _userService.GetUserByEmail(TokenHelper.GetEmail(bearerToken));
        try
        {
            if (TokenHelper.GetEmail(bearerToken) == user.Email)
            {
                await _userService.UpdateUser(user);
                return Ok();
            }
            else
            {
                throw new Exception();
            }
        }
        catch
        {
            return BadRequest("Ошибка обновления");
        }
    }

    [HttpGet("NewToken")]
    [Authorize]
    public async Task<IActionResult> GetNewUserToken()
    {
        var bearerToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        User? userFromDb = await _userService.GetUserByEmail(TokenHelper.GetEmail(bearerToken));
        if (userFromDb != null)
            return Ok(_tokenService.CreateToken(userFromDb));
        else
            return BadRequest("Ошибка создание токена");
    }

    [HttpGet("GetDistributorQrs")]
    [Authorize]
    public async Task<IActionResult> GetDistributorQrs()
    {
        var bearerToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        try
        {
            string id = TokenHelper.GetNameIdentifer(bearerToken);
            return Ok(await _userService.GetWorkersDistributorById(int.Parse(id)));
        }
        catch
        {
            return BadRequest("Ошибка");
        }
    }

    [HttpGet("GetClients")]
    [Authorize]
    public async Task<IActionResult> GetClients()
    {
        var bearerToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        try
        {
            string id = TokenHelper.GetNameIdentifer(bearerToken);
            return Ok(await _userService.GetClients(int.Parse(id)));
        }
        catch
        {
            return BadRequest("Ошибка");
        }
    }

    [HttpGet("GetWorkerQrs")]
    [Authorize]
    public async Task<IActionResult> GetWorkerQrs()
    {
        var bearerToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        try
        {
            string id = TokenHelper.GetNameIdentifer(bearerToken);
            return Ok(await _userService.GetWorkerQrs(int.Parse(id)));
        }
        catch
        {
            return BadRequest("Ошибка");
        }
    }

    /// <summary>
    /// Получение распространителей, которые привязаны к поставщику услуг
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetDistributorsWorker")]
    [Authorize]
    public async Task<IActionResult> GetDistributorsWorker()
    {
        var bearerToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        try
        {
            string id = TokenHelper.GetNameIdentifer(bearerToken);
            return Ok(await _userService.GetDistributorsWorker(int.Parse(id)));
        }
        catch
        {
            return BadRequest("Ошибка");
        }
    }


    /// <summary>
    /// Получение распространителей, которые привязаны к поставщику услуг
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetDistributorsClient")]
    [Authorize]
    public async Task<IActionResult> GetDistributorsClient()
    {
        var bearerToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        try
        {
            string id = TokenHelper.GetNameIdentifer(bearerToken);
            return Ok(await _userService.GetDistributorsClient(int.Parse(id)));
        }
        catch
        {
            return BadRequest("Ошибка");
        }
    }

    /// <summary>
    /// Получение истории распрсотранителей, которые связаны с поставщиком услуг
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetHistoryDistributors")]
    [Authorize]
    public async Task<IActionResult> GetHistoryDistributors()
    {
        var bearerToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        try
        {
            string id = TokenHelper.GetNameIdentifer(bearerToken);
            return Ok(await _userService.GetHistoryDistributors(int.Parse(id)));
        }
        catch
        {
            return BadRequest("Ошибка");
        }
    }

    /// <summary>
    /// Получение истории клиентов, которые распрстраняют ваш qr код
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetHistoryClients")]
    [Authorize]
    public async Task<IActionResult> GetHistoryClients()
    {
        var bearerToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        try
        {
            string id = TokenHelper.GetNameIdentifer(bearerToken);
            return Ok(await _userService.GetHistoryClients(int.Parse(id)));
        }
        catch
        {
            return BadRequest("Ошибка");
        }
    }

    /// <summary>
    /// Получение истории пользователя о применении Qr кодов распрсотранителей
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetHistoryUserQrApplied")]
    [Authorize]
    public async Task<IActionResult> GetHistoryUserQrApplied()
    {
        var bearerToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        try
        {
            string id = TokenHelper.GetNameIdentifer(bearerToken);
            return Ok(await _userService.GetHistoryUserQrApplied(int.Parse(id)));
        }
        catch
        {
            return BadRequest("Ошибка");
        }
    }

    [HttpGet("CheckExistUserByEmail")]
    public async Task<IActionResult> CheckExistUserByEmail(string email)
    {
        if (await _userService.CheckExistUser(email))
            return Ok();
        return NoContent();
    }
}