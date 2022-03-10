namespace Shreco.API.Interfaces;
public interface IQrService {
    Task AddQr(Qr? qr);
    Task<Qr> GetQrById(int id);
    Task UpdateQr(params Qr[] qrs);
    Task<bool> IsExistQrClient(int clientId, int distibutorId, int workerId);
    Task<bool> IsExistRegistartionQr(int percent, int percentClient, int workerId);
}