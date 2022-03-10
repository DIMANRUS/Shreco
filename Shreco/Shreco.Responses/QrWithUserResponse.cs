using System.Text.Json.Serialization;
using Shreco.Models;

namespace Shreco.Responses {
    public class QrWithUserResponse {
        [JsonPropertyName("qr")]
        public Qr Qr { get; set; }
        [JsonPropertyName("user")]
        public User User { get; set; }
    }
}