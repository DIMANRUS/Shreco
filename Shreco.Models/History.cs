using System;
using System.Text.Json.Serialization;

namespace Shreco.Models;

public class History : BaseModel {
    [JsonPropertyName("qrId")]
    public int QrId { get; set; }

    [JsonIgnore]
    public Qr Qr { get; set; }

    [JsonPropertyName("dateApplied")]
    public DateTime DateApplied { get; set; } = DateTime.Now;

    [JsonPropertyName("price")]
    public float Price { get; set; }

    [JsonPropertyName("isConfirm")]
    public bool IsConfirm { get; set; }
}