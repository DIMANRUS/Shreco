using System.Text.Json.Serialization;
using Shreco.Models;

namespace Shreco.Responses {
    public class HistoryWithQrUserResponse : QrWithUserResponse {
        [JsonPropertyName("history")]
        public History History { get; set; }
        [JsonPropertyName("client")]
        public User Client { get; set; }
    }
}