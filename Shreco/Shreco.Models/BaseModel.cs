using System.ComponentModel.DataAnnotations.Schema;

namespace Shreco.Models;

public abstract class BaseModel {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
}