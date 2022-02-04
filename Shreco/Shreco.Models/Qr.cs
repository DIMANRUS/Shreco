namespace Shreco.Models {
    public class Qr : BaseModel {
        public QrType QrType { get; set; }
        public User WhoCreated { get; set; }
        public User ForWhoCreated { get; set; }
    }

    public enum QrType {
        Registration, Bonus
    }
}