namespace Shreco.API.Controllers;

[ApiController]
[Route("[controller]")]
public class QrController : ControllerBase {
    #region Private fields
    private readonly IQrService _qrService;
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    private readonly IHistoryService _historyService;
    private readonly AppContext _appContext;
    #endregion

    public QrController(IQrService qrService, ITokenService tokenService, AppContext appContext,
                        IUserService userService, IHistoryService historyService)
    {
        _qrService = qrService;
        _tokenService = tokenService;
        _appContext = appContext;
        _userService = userService;
        _historyService = historyService;
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
            string bearerToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
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
            string bearerToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            Qr qr = await _qrService.GetQrById(qrId);
            if (TokenHelper.GetNameIdentifer(bearerToken) == qr.WorkerId.ToString())
                throw new Exception();
            qr.DistributorId = int.Parse(TokenHelper.GetNameIdentifer(bearerToken));
            qr.QrType = QrType.Distibutor;
            await _qrService.UpdateQr(qr);
            return Ok("QR код добавлен");
        } catch {
            return BadRequest("Ошибка создания Qr.");
        }
    }

    [HttpGet("AddDistributorToClient")]
    [Authorize]
    public async Task<IActionResult> AddDIstributorToClient(int qrId)
    {
        try {
            string bearerToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            Qr qr = await _qrService.GetQrById(qrId);
            int clientId = int.Parse(TokenHelper.GetNameIdentifer(bearerToken));
            if (await _qrService.IsExistQrClient(clientId, qr.DistributorId, qr.WorkerId) ||
                qr.DistributorId.ToString() == TokenHelper.GetNameIdentifer(bearerToken))
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

    /// <summary>
    /// Вызывается при сканировнии работником Qr кода клиента
    /// </summary>
    /// <param name="qrId"></param>
    /// <returns></returns>
    [HttpPost("WorkerFromQrCLient")]
    [Authorize]
    public async Task<IActionResult> WorkerFromQrClient(ClientQrAfterScaningRequest request)
    {
        try {
            string bearerToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            Qr qr = await _qrService.GetQrById(request.QrId);
            if (qr.WorkerId != int.Parse(TokenHelper.GetNameIdentifer(bearerToken)))
                return BadRequest("Qr недействительный");
            History history = new() {
                QrId = request.QrId,
                Price = request.Price
            };
            await _historyService.AddHistory(history);
            return Ok();
        } catch {
            return BadRequest("Ошибка");
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
            string bearerToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            return Ok(_tokenService.CreateToken(qr));
        } catch {
            return BadRequest("Ошибка создания токена");
        }
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> RemoveQr(int qrId)
    {
        try {
            string bearerToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            Qr qr = await _qrService.GetQrById(qrId);
            int userId = int.Parse(TokenHelper.GetNameIdentifer(bearerToken));
            if (qr.WorkerId == userId || qr.ClientId == userId || qr.DistributorId == userId)
                await _qrService.RemoveQr(qr);
            return Ok();
        } catch {
            return BadRequest("Ошибка удаления");
        }
    }
}