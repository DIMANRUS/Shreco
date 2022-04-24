using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Shreco.Models;

public abstract class BaseModel {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [JsonPropertyName("id")]
    public int Id { get; set; }
}