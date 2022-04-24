namespace Shreco.API.Services;

public class UserService : IUserService
{
    private readonly AppContext _appContext;
    public UserService(AppContext appContext) =>
        _appContext = appContext;

    public async Task AddUsers(params User[] users)
    {
        await _appContext.Users.AddRangeAsync(users);
        await _appContext.SaveChangesAsync();
    }

    public async Task<User?> GetUserByEmail(string mail) =>
        await _appContext.Users.AsNoTracking()
            .SingleOrDefaultAsync(x => x.Email == mail);

    public async Task<User?> GetUserById(int id) =>
        await _appContext.Users.AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == id);

    /// <summary>
    /// Получение поставщиков услуг, которых распространяет распространитель
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IEnumerable<QrWithUserResponse>> GetWorkersDistributorById(int id)
    {
        IEnumerable<Qr> workers = await _appContext.Qrs.AsNoTracking().Where(x => x.DistributorId == id && x.QrType == QrType.Distibutor).ToListAsync();
        List<QrWithUserResponse> response = new();
        foreach (var qr in workers)
        {
            response.Add(new()
            {
                Qr = qr,
                User = await GetUserById(qr.WorkerId)
            });
        }
        return response;
    }

    /// <summary>
    /// Метод возвращающий список клиентов, которые отсканировали qr код распрсотранителя
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<IEnumerable<QrWithUserResponse>> GetClients(int id)
    {
        IEnumerable<Qr> clients = await _appContext.Qrs.AsNoTracking().Where(x => x.DistributorId == id && x.QrType == QrType.Client).ToListAsync();
        List<QrWithUserResponse> response = new();
        foreach (var qr in clients)
        {
            response.Add(new()
            {
                Qr = qr,
                User = await GetUserById(qr.ClientId)
            });
        }
        return response;
    }

    /// <summary>
    /// Получения Qr кодов, выпущенных поставщиком услуг, но ещё не применены распрсотранителем
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Qr>> GetWorkerQrs(int id)
    {
        IEnumerable<Qr> workerQrs = await _appContext.Qrs.AsNoTracking().Where(x => x.WorkerId == id && x.QrType == QrType.Registration).ToListAsync();
        return workerQrs;
    }

    /// <summary>
    /// Получение распространителей, которые привязаны к поставщику услуг
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IEnumerable<QrWithUserResponse>> GetDistributorsWorker(int id)
    {
        IEnumerable<Qr> distributorsWorker = await _appContext.Qrs.AsNoTracking().Where(x => x.WorkerId == id && x.QrType == QrType.Distibutor).ToListAsync();
        List<QrWithUserResponse> response = new();
        foreach (var qr in distributorsWorker)
        {
            response.Add(new()
            {
                Qr = qr,
                User = await GetUserById(qr.DistributorId)
            });
        }
        return response;
    }

    /// <summary>
    /// Получение распространителей, которые привязаны к поставщику услуг
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IEnumerable<QrWithUserResponse>> GetDistributorsClient(int id)
    {
        IEnumerable<Qr> distributorsWorker = await _appContext.Qrs.AsNoTracking().Where(x => x.ClientId == id && x.QrType == QrType.Client).ToListAsync();
        List<QrWithUserResponse> response = new();
        foreach (var qr in distributorsWorker)
        {
            response.Add(new()
            {
                Qr = qr,
                User = await GetUserById(qr.DistributorId)
            });
        }
        return response;
    }

    /// <summary>
    /// Получение истории распрсотранителей, которые связаны с поставщиком услуг
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IEnumerable<HistoryWithQrUserResponse>> GetHistoryDistributors(int id)
    {
        IEnumerable<History> histories = await _appContext.Histories.AsNoTracking().Include(x => x.Qr).Where(x => x.Qr.WorkerId == id && x.Qr.QrType == QrType.Client).ToListAsync();
        List<HistoryWithQrUserResponse> response = new();
        foreach (var history in histories)
        {
            response.Add(new()
            {
                Qr = history.Qr,
                History = history,
                User = await GetUserById(history.Qr.DistributorId),
                Client = await GetUserById(history.Qr.ClientId)
            });
        }
        return response;
    }

    /// <summary>
    /// Получение истории клиентов, которые распрстраняют ваш qr код
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IEnumerable<HistoryWithQrUserResponse>> GetHistoryClients(int id)
    {
        IEnumerable<History> histories = await _appContext.Histories.AsNoTracking().Include(x => x.Qr).Where(x => x.Qr.DistributorId == id && x.Qr.QrType == QrType.Client).ToListAsync();
        List<HistoryWithQrUserResponse> response = new();
        foreach (var history in histories)
        {
            response.Add(new()
            {
                Qr = history.Qr,
                History = history,
                User = await GetUserById(history.Qr.WorkerId),
                Client = await GetUserById(history.Qr.ClientId)
            });
        }
        return response;
    }

    /// <summary>
    /// Получение истории пользователя о применении Qr кодов распрсотранителей
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IEnumerable<HistoryWithQrUserResponse>> GetHistoryUserQrApplied(int id)
    {
        IEnumerable<History> histories = await _appContext.Histories.AsNoTracking().Include(x => x.Qr).Where(x => x.Qr.ClientId == id && x.Qr.QrType == QrType.Client).ToListAsync();
        List<HistoryWithQrUserResponse> response = new();
        foreach (var history in histories)
        {
            response.Add(new()
            {
                Qr = history.Qr,
                History = history,
                User = await GetUserById(history.Qr.WorkerId),
                Client = await GetUserById(history.Qr.ClientId)
            });
        }
        return response;
    }

    public async Task<bool> CheckExistUser(string email)
    {
        return await _appContext.Users.FirstOrDefaultAsync(x => x.Email == email) != null;
    }

    public async Task UpdateUser(User user)
    {
        _appContext.Users.Update(user);
        await _appContext.SaveChangesAsync();
    }
}