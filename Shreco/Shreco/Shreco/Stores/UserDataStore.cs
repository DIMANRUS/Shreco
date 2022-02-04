using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Shreco.Stores {
    internal class UserDataStore {
        public static async Task Initializate() {
            Token = await SecureStorage.GetAsync("UserToken");
        }
        public static string Token { get; set; }
    }
}