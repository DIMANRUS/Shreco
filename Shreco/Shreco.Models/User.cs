using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shreco.Models;

public class User : BaseModel {
    [StringLength(20)]
    public string NameIdentifer { get; set; }
    [Required]
    [StringLength(50)]
    [RegularExpression(@"^\S+@\S+\.\S+$")]
    public string Email { get; set; }
    [StringLength(100)]
    public string? Adress { get; set; }
    [StringLength(30)]
    public string? Phone { get; set; }

    public ICollection<Qr> QrsWhoCreated { get; set; } = new List<Qr>();
    public ICollection<Qr> QrsForWhoCreated { get; set; } = new List<Qr>();
    public ICollection<History> HistoriesWhoApplied { get; set; } = new List<History>();
    public ICollection<History> HistoriesWhoUsed { get; set; } = new List<History>();
}