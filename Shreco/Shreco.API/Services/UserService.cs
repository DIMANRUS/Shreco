namespace Shreco.API.Services;

public class UserService : IUserService {
    private readonly AppContext _appContext;
    public UserService(AppContext appContext) =>
        _appContext = appContext;

    public async Task AddUser(User user) =>
        await _appContext.Users.AddAsync(user);

    public Task<User> GetUser(string mail) {
        throw new NotImplementedException();
    }

    public async Task SaveChanges() =>
        await _appContext.SaveChangesAsync();

    public async Task<bool> UserExist(string mail) {
        User? user = await _appContext.Users.FirstOrDefaultAsync(x => x.Email == mail);
        return user != null;
    }
}