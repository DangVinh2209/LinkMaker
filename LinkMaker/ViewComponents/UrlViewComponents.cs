using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LinkMaker.Common.DTOs;
using LinkMaker.Data;
using LinkMaker.Common.DTOS;

namespace LinkMaker.MVC.ViewComponents
{
    public class UrlViewComponent : ViewComponent
    {
        private readonly LinkMakerDbContext _context;

        public UrlViewComponent(LinkMakerDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var urls = await _context.Urls
                .Select(m => new UrlDTO
                {
                    Id = m.Id,
                    YourLink = m.YourLink,
                    NewLink = m.NewLink,
                   UrlCode = m.UrlCode,
                })
                .ToListAsync();
            return View(urls); // Trả về View Default.cshtml
        }


    }
}
