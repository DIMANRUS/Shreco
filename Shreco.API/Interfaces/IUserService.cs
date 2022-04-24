namespace Shreco.API.Interfaces;

public interface IUserService
{
    Task AddUsers(params User[] users);
    Task<bool> CheckExistUser(string email);
    Task<User?> GetUserByEmail(string mail);
    Task<User?> GetUserById(int id);
    Task UpdateUser(User user);
    Task<IEnumerable<QrWithUserResponse>> GetWorkersDistributorById(int id);
    Task<IEnumerable<QrWithUserResponse>> GetClients(int id);
    Task<IEnumerable<Qr>> GetWorkerQrs(int id);
    Task<IEnumerable<QrWithUserResponse>> GetDistributorsWorker(int id);
    Task<IEnumerable<QrWithUserResponse>> GetDistributorsClient(int id);
    Task<IEnumerable<HistoryWithQrUserResponse>> GetHistoryDistributors(int id);
    Task<IEnumerable<HistoryWithQrUserResponse>> GetHistoryClients(int id);
    Task<IEnumerable<HistoryWithQrUserResponse>> GetHistoryUserQrApplied(int id);
}