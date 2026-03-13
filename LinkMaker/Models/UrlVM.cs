namespace LinkMaker.MVC.Models
{
    public class UrlVM
    {
        public int Id { get; set; }
        public required string YourUrl { get; set; }
        public required string NewUrl { get; set; }
    }
}
