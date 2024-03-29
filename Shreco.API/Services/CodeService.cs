﻿namespace Shreco.API.Services;

public class CodeService : ControllerBase, ICodeService {
    #region Fields
    private readonly AppContext _appContext;
    private readonly IMailService _mailService;
    private readonly ITokenService _tokenService;
    #endregion
    public CodeService(AppContext liteContext, IMailService mailService, ITokenService tokenService)
    {
        _appContext = liteContext;
        _mailService = mailService;
        _tokenService = tokenService;
    }
    #region Private Methods
    private static string GenerateCode()
    {
        Random random = new();
        string code = "";
        for (int i = 0; i < 4; i++)
            code += random.Next(9);
        return code;
    }
    private static string GenerateSessionCode()
    {
        Random random = new();
        string code = "";
        for (int i = 0; i < 4; i++)
            code += random.Next(100);
        return code;
    }
    private async Task RemoveSession(Session session)
    {
        _appContext.Remove(session);
        await _appContext.SaveChangesAsync();
    }
    #endregion
    #region Public Methods
    public async Task<ObjectResult> SendCode(string mail)
    {
        //try {
#if DEBUG
        Session session = new() {
            Code = "0000",
            SessionId = GenerateSessionCode()
        };
#else
            Session session = new() {
                Code = GenerateCode(),
                SessionId = GenerateSessionCode()
            };
#endif
        await _appContext.Sessions.AddAsync(session);
        await _appContext.SaveChangesAsync();
        await _mailService.SendMailWithCode(mail, session.Code);
        return Ok(_tokenService.CreateToken(session));
        //} catch {
        //    return BadRequest("Failed generate");
        //}
    }
    public async Task<bool> CheckValidCode(string sessionId, string userCode)
    {
        IEnumerable<Session> sessions = await _appContext.Sessions.ToListAsync();
        Session? session = await _appContext.Sessions.FirstOrDefaultAsync(x => x.SessionId == sessionId);
        if (session == null)
            return false;
        await RemoveSession(session);
        return session.Code == userCode;
    }

    #endregion
}