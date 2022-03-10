namespace Shreco.API.Controllers;
[ApiController]
[Route("[controller]")]
public class QrController : ControllerBase {
    #region
    private readonly IQrService _qrService;
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    private readonly AppContext _appContext;
    #endregion
    public QrController(IQrService qrService, ITokenService tokenService, AppContext appContext, IUserService userService)
    {
        _qrService = qrService;
        _tokenService = tokenService;
        _appContext = appContext;
        _userService = userService;
    }

    /// <summary>
    /// Метод добавления QR кода регистрации, выпущенного поставщиком услуг для распространителя
    /// </summary>
    /// <param name="percent"></param>
    /// <param name="percentForClient"></param>
    /// <returns></returns>
    [HttpGet("AddRegQr")]
    [Authorize]
    public async Task<IActionResult> AddRegQr(string percent, string percentForClient)
    {
        try {
            var bearerToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            if (await _qrService.IsExistRegistartionQr(int.Parse(percent), int.Parse(percentForClient),
                    int.Parse(TokenHelper.GetNameIdentifer(bearerToken))))
                return BadRequest("У вас уже есть Qr код с такими процентами.");
            Qr qr = new() {
                WorkerId = int.Parse(TokenHelper.GetNameIdentifer(bearerToken)),
                QrType = QrType.Registration,
                Percent = int.Parse(percent),
                PercentForClient = int.Parse(percentForClient)
            };
            await _qrService.AddQr(qr);
            return Ok(_tokenService.CreateToken(qr));
        } catch {
            return BadRequest("Ошибка создания Qr.");
        }
    }

    [HttpGet("AddWorkerToDistributor")]
    [Authorize]
    public async Task<IActionResult> AddWorkerToDistributor(int qrId)
    {
        try {
            var bearerToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            Qr qr = await _qrService.GetQrById(qrId);
            if (qr == null || TokenHelper.GetNameIdentifer(bearerToken) == qr.WorkerId.ToString())
                throw new Exception();
            qr.DistributorId = int.Parse(TokenHelper.GetNameIdentifer(bearerToken));
            qr.QrType = QrType.Distibutor;
            await _qrService.UpdateQr(qr);
            return Ok("QR код добавлен");
        } catch {
            return BadRequest("Ошибка создания Qr.");
        }
    }

    [HttpGet("AddDIstributorToClient")]
    [Authorize]
    public async Task<IActionResult> AddDIstributorToClient(int qrId)
    {
        try {
            var bearerToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            Qr qr = await _qrService.GetQrById(qrId);
            int clientId = int.Parse(TokenHelper.GetNameIdentifer(bearerToken));
            if (qr == null || await _qrService.IsExistQrClient(clientId, qr.DistributorId, qr.WorkerId) || qr.DistributorId.ToString() == TokenHelper.GetNameIdentifer(bearerToken))
                throw new Exception();
            Qr clientQr = new() {
                ClientId = clientId,
                WorkerId = qr.WorkerId,
                DistributorId = qr.DistributorId,
                Percent = qr.Percent,
                PercentForClient = qr.PercentForClient,
                QrType = QrType.Client
            };
            await _qrService.AddQr(clientQr);
            return Ok("Qr код добавлен");
        } catch {
            return BadRequest("Ошибка создания Qr.");
        }
    }

    [HttpGet("GetQrToken")]
    [Authorize]
    public async Task<IActionResult> GetQrToken(int qrId)
    {
        try {
            Qr qr = await _appContext.Qrs.SingleOrDefaultAsync(x => x.Id == qrId);
            if (qr == null)
                throw new Exception();
            var bearerToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            return Ok(_tokenService.CreateToken(qr));
        } catch {
            return BadRequest("Ошибка создания токена");
        }
    }
}