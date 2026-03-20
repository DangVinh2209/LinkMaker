namespace LinkMaker.Data.Entities
{
    public class QRCode
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string URL { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
    }
}
