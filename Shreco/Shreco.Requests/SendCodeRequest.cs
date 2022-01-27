using System;

namespace Shreco.Requests {
    public class SendCodeRequest {
        [Required]
        [StringLength(50)]
        [RegularExpression(@"^\S+@\S+\.\S+$")]
        public string Email { get; set; }
    }
}
