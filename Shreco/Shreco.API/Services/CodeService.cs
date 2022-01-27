namespace Shreco.API.Services;

public class CodeService : ControllerBase, ICodeService {
    #region Fields
    private readonly LiteContext _liteContext;
    private readonly IMailService _mailService;
    private readonly ITokenService _tokenService;
    #endregion
    public CodeService(LiteContext liteContext, IMailService mailService, ITokenService tokenService) {
        _liteContext = liteContext;
        _mailService = mailService;
        _tokenService = tokenService;
    }
    #region Private Methods
    private static string GenerateCode() {
        Random random = new();
        string code = "";
        for (int i = 0; i < 4; i++)
            code += random.Next(9);
        return code;
    }
    private static string GenerateSessionCode() {
        Random random = new();
        string code = "";
        for (int i = 0; i < 4; i++)
            code += random.Next(100);
        return code;
    }
    private async Task RemoveSession(string sessionId) {
        Session? session = await _liteContext.Sessions.AsNoTracking().FirstOrDefaultAsync(x => x.SessionId == sessionId);
        if (session != null)
            _liteContext.Remove(session);
        await _liteContext.SaveChangesAsync();
    }
    #endregion
    #region Public Methods
    public async Task<ObjectResult> SendCode(string mail) {
        try {
            Session session = new() {
                Code = GenerateCode(),
                SessionId = GenerateSessionCode()
            };
            await _liteContext.Sessions.AddAsync(session);
            await _liteContext.SaveChangesAsync();
            await _mailService.SendMailWithCode(mail, session.Code);
            return Ok(_tokenService.CreateSessionToken(session));
        } catch {
            return BadRequest("Failed generate");
        }
    }
    public async Task<bool> CheckValidCode(string sessionId, string userCode) {
        Session? session = await _liteContext.Sessions.AsNoTracking().FirstOrDefaultAsync(x => x.SessionId == sessionId);
        if (session == null)
            return false;
        await RemoveSession(sessionId);
        return session.Code == userCode;
    }

    #endregion
}