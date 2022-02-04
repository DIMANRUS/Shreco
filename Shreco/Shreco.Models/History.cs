using System;

namespace Shreco.Models {
    public class History : BaseModel {
        public Qr Qr { get; set; }
        public User WhoApplied { get; set; }
        public User WhoUsed { get; set; }
        public DateTime DateApplied { get; set; }
        public float Price { get; set; }
    }
}