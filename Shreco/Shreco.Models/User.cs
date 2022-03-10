using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shreco.Models;

public class User : BaseModel {
    [StringLength(20)]
    [JsonPropertyName("nameIdentifer")]
    public string NameIdentifer { get; set; }
    [JsonPropertyName("email")]
    [Required]
    [StringLength(50)]
    [RegularExpression(@"^\S+@\S+\.\S+$")]
    public string Email { get; set; }
    [JsonPropertyName("adress")]
    [StringLength(100)]
    public string? Adress { get; set; } = "";

    [StringLength(30)]
    [JsonPropertyName("phone")]
    public string? Phone { get; set; } = "";
    [JsonPropertyName("money")]
    public double Money { get; set; }
}