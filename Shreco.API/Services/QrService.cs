﻿namespace Shreco.API.Services;
public class QrService : ControllerBase, IQrService {
    #region Private fields
    private readonly AppContext _context;
    #endregion
    public QrService(AppContext context) {
        _context = context;
    }
    public async Task AddQr(Qr qr) {
        await _context.Qrs.AddAsync(qr);
        await _context.SaveChangesAsync();
    }
    public async Task<Qr> GetQrById(int id) =>
        await _context.Qrs.AsNoTracking().SingleAsync(x =>
            x.Id == id);

    public async Task UpdateQr(params Qr[] qrs) {
        _context.Qrs.UpdateRange(qrs);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsExistQr(int qrId) =>
        await _context.Qrs.SingleOrDefaultAsync(x =>
            x.Id == qrId) != null;

    public async Task<bool> IsExistQrClient(int clientId, int distibutorId, int workerId) =>
        await _context.Qrs.SingleOrDefaultAsync(x =>
            x.ClientId == clientId && x.DistributorId == distibutorId && x.WorkerId == workerId) != null;

    public async Task<bool> IsExistRegistartionQr(int percent, int percentClient, int workerId) =>
        await _context.Qrs.SingleOrDefaultAsync(x =>
              x.Percent == percent && x.PercentForClient == percentClient && x.WorkerId == workerId) != null;
    public async Task RemoveQr(Qr qr)
    {
        _context.Remove(qr);
        await _context.SaveChangesAsync();
    }
}