namespace Shreco.API.Interfaces {
    public interface IUserService {
        Task AddUser(User user);
        Task<bool> UserExist(string mail);
        Task<User> GetUser(string mail);
        Task SaveChanges();
    }
}