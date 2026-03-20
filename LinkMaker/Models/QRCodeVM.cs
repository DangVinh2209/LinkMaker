using Microsoft.AspNetCore.Mvc;

namespace LinkMaker.MVC.Models
{
    //[Bind("URL")]
    public class QRCodeVM
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string URL { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
    }
}
