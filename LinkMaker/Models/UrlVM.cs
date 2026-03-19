using LinkMaker.Common.DTOS;

namespace LinkMaker.MVC.Models
{
    public class UrlVM
    {
        public UrlVM()
        {
        }

        public UrlVM(UrlDTO dto)
        {
            Id = dto.Id;
            YourLink = dto.YourLink;
            NewLink = dto.NewLink;
            //UrlCode = dto.UrlCode;
        }

        public Guid Id { get; set; }

        public string YourLink { get; set; } = string.Empty;

        public string NewLink { get; set; } = string.Empty;

        //public string? UrlCode { get; set; }
    }
}