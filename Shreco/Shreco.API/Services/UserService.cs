namespace Shreco.API.Services;

public class UserService : IUserService {
    private readonly AppContext _appContext;
    public UserService(AppContext appContext) =>
        _appContext = appContext;

    public async Task AddUser(User user) =>
        await _appContext.Users.AddAsync(user);

    public async Task<User> GetUser(string mail) =>
        await _appContext.Users.SingleOrDefaultAsync(x => x.Email == mail);

    public async Task SaveChanges() =>
        await _appContext.SaveChangesAsync();

    public async Task<bool> UserExist(string mail)
    {
        User? user = await _appContext.Users.FirstOrDefaultAsync(x => x.Email == mail);
        return user != null;
    }
}