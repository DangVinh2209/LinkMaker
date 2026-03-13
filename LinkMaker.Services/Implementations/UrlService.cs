using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LinkMaker.Common.DTOs;
using LinkMaker.Common.DTOS;
using LinkMaker.Data;
using LinkMaker.Data.Entities;
using LinkMaker.Data.Interfaces;
namespace LinkMaker.Services.Implementations
{
    public class UrlService : IUrlService
    {
        private readonly LinkMakerDbContext _context;
        public UrlService(LinkMakerDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(UrlDTO dtoUrl)
        {
            var isOK = false;
            try
            {
                var newUrl = new Url
                {
                    YourLink = dtoUrl.YourLink.Trim(),
                    //Description = dtoUrl.Description?.Trim(),
                    UrlCode = dtoUrl.UrlCode?.Trim(),
                };
                await _context.Urls.AddAsync(newUrl);
                await _context.SaveChangesAsync();
                isOK = true;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here if needed
                isOK = false;
            }
            return isOK;
        }

    }
}
