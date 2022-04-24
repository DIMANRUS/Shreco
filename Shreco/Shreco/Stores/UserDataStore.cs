namespace Shreco.Stores;

public class UserDataStore {
    public static async Task Set(DatasNames dataName, object value) =>
        await SecureStorage.SetAsync(dataName.ToString(), value.ToString());
    public static async Task<string> Get(DatasNames dataName) =>
        await SecureStorage.GetAsync(dataName.ToString());

    public static void Clear() =>
         SecureStorage.RemoveAll();
}
public enum DatasNames {
    Token
}