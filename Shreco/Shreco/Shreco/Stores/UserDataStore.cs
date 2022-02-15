namespace Shreco.Stores;

public class UserDataStore {
    public static async Task Initializate() {
        Token = await SecureStorage.GetAsync("UserToken");
    }
    public static string Token { get; set; }
}