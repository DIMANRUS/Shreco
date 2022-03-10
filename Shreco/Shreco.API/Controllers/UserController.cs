namespace Shreco.API.Controllers;
[Route("[controller]")]
[ApiController]
public class UserController : ControllerBase {
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    [HttpGet("GetDistributorQrs")]
    [Authorize]
    public async Task<IActionResult> GetDistributorQrs()
    {
        var bearerToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        try {
            string id = TokenHelper.GetNameIdentifer(bearerToken);
            return Ok(await _userService.GetWorkersDistributorById(int.Parse(id)));
        } catch {
            return BadRequest("Ошибка");
        }
    }

    [HttpGet("GetClients")]
    [Authorize]
    public async Task<IActionResult> GetClients()
    {
        var bearerToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        try {
            string id = TokenHelper.GetNameIdentifer(bearerToken);
            return Ok(await _userService.GetClients(int.Parse(id)));
        } catch {
            return BadRequest("Ошибка");
        }
    }

    [HttpGet("GetWorkerQrs")]
    [Authorize]
    public async Task<IActionResult> GetWorkerQrs()
    {
        var bearerToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        try {
            string id = TokenHelper.GetNameIdentifer(bearerToken);
            return Ok(await _userService.GetWorkerQrs(int.Parse(id)));
        } catch {
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
        try {
            string id = TokenHelper.GetNameIdentifer(bearerToken);
            return Ok(await _userService.GetDistributorsWorker(int.Parse(id)));
        } catch {
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
        try {
            string id = TokenHelper.GetNameIdentifer(bearerToken);
            return Ok(await _userService.GetDistributorsClient(int.Parse(id)));
        } catch {
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
        try {
            string id = TokenHelper.GetNameIdentifer(bearerToken);
            return Ok(await _userService.GetHistoryDistributors(int.Parse(id)));
        } catch {
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
        try {
            string id = TokenHelper.GetNameIdentifer(bearerToken);
            return Ok(await _userService.GetHistoryClients(int.Parse(id)));
        } catch {
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
        try {
            string id = TokenHelper.GetNameIdentifer(bearerToken);
            return Ok(await _userService.GetHistoryUserQrApplied(int.Parse(id)));
        } catch {
            return BadRequest("Ошибка");
        }
    }
}