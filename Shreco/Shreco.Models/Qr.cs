using System;
using System.Text.Json.Serialization;

namespace Shreco.Models;

public class Qr : BaseModel {
    [JsonPropertyName("distributorId")]
    public int DistributorId { get; set; }
    [JsonPropertyName("workerId")]
    public int WorkerId { get; set; }
    public int ClientId { get; set; }
    [JsonPropertyName("percent")]
    public int Percent { get; set; }
    [JsonPropertyName("percentForClient")]
    public int PercentForClient { get; set; }
    [JsonPropertyName("qrType")]
    public QrType QrType { get; set; } = QrType.Registration;
    [JsonPropertyName("dateCreated")]
    public DateTime DateCreated { get; set; } = DateTime.Now;
}

public enum QrType {
    Registration, Distibutor, Client
}