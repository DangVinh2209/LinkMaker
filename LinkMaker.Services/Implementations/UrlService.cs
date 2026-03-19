using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LinkMaker.Common.DTOs;
using LinkMaker.Common.DTOS;
using LinkMaker.Data;
using LinkMaker.Data.Entities;
using LinkMaker.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
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
                    //UrlCode = dtoUrl.UrlCode?.Trim(),
                    UserId = dtoUrl.UserId,
                    NewLink = dtoUrl.NewLink?.Trim() ?? string.Empty
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
        public async Task<bool> Delete(Guid idUrl)
        {
            try
            {
                var url = await _context.Urls.FindAsync(idUrl);
                if (url == null)
                {
                    return false; // Item already gone or not found
                }

                _context.Urls.Remove(url);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // This will help you see the REAL error in the Output window
                //System.Diagnostics.Debug.WriteLine(ex.Message);
                //return false;
                throw; // Rethrow the exception to be handled by the caller
            }
        }
        public async Task<UrlDTO[]?> GetAll()
        {
            try
            {
                var majors = await _context.Urls
                        .Select(x => new UrlDTO
                        {
                            Id = x.Id,
                            YourLink = x.YourLink,
                            NewLink = x.NewLink,
                            //UrlCode = x.UrlCode,
                            UserId = x.UserId,
                        })
                        .ToArrayAsync();

                // Ensure we return an empty array instead of null if the query result is empty
                return majors ?? Array.Empty<UrlDTO>();
            }
            catch (Exception ex)
            {
                // Log your error here
                return Array.Empty<UrlDTO>(); // Return empty on failure
            }
        }
        public async Task<UrlDTO?> GetById(Guid idUrl)
        {
            try
            {
                var majorDTO = await _context.Urls
                .Where(m => m.Id == idUrl)
                .Select(m => new UrlDTO
                {
                    Id = m.Id,
                    YourLink = m.YourLink,
                    NewLink = m.NewLink,
                    //UrlCode = m.UrlCode,
                    UserId = m.UserId,
                })
                .SingleOrDefaultAsync();
                return majorDTO;
            }
            catch (Exception ex)
            {

                throw;
            }
            return null;
        }
        public async Task<bool> Update(UrlDTO urlDTO)
        {
            var isOK = false;
            try
            {
                var url = await _context.Urls
                        //.Where(m => m.Id == majorDTO.Id)
                        .Where(m => m.Id.Equals(urlDTO.Id))
                        .SingleOrDefaultAsync();
                if (url != null)
                {
                    url.YourLink = urlDTO.YourLink.Trim();
                    url.NewLink = urlDTO.NewLink?.Trim();
                    //url.UrlCode = urlDTO.UrlCode?.Trim();
                }
                await _context.SaveChangesAsync();
                isOK = true;
            }
            catch (Exception ex)
            {
            }
            return isOK;

        }
        public async Task<UrlDTO[]?> GetByUserId(Guid userId)
        {
            try
            {
                var urls = await _context.Urls
                    .Where(x => x.UserId == userId)
                    .Select(x => new UrlDTO
                    {
                        Id = x.Id,
                        YourLink = x.YourLink,
                        NewLink = x.NewLink,
                        //UrlCode = x.UrlCode,
                        UserId = x.UserId
                    })
                    .ToArrayAsync();

                return urls ?? Array.Empty<UrlDTO>();
            }
            catch
            {
                return Array.Empty<UrlDTO>();
            }
        }

        public async Task<UrlDTO?> GetById(Guid idUrl, Guid userId)
        {
            try
            {
                return await _context.Urls
                    .Where(x => x.Id == idUrl && x.UserId == userId)
                    .Select(x => new UrlDTO
                    {
                        Id = x.Id,
                        YourLink = x.YourLink,
                        NewLink = x.NewLink,
                        //UrlCode = x.UrlCode,
                        UserId = x.UserId
                    })
                    .SingleOrDefaultAsync();
            }
            catch
            {
                return null;
            }
        }
    }
}
