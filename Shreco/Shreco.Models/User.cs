using System.ComponentModel.DataAnnotations;

namespace Shreco.Models {
    public class User : BaseModel {
        [Required]
        [StringLength(20)]
        public string NameIdentifer { get; set; }
        [Required]
        [StringLength(50)]
        [RegularExpression(@"^\S+@\S+\.\S+$")]
        public string Email { get; set; }
        [StringLength(100)]
        public string? Adress { get; set; }
    }
}