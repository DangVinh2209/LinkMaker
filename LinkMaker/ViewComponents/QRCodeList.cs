using LinkMaker.Common.Contants;
using LinkMaker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using LinkMaker.Data;
using System.Text.Json;
using System.Linq;
using QRCoder;

namespace LinkMaker.MVC.ViewComponents
{
    public class QRCodeList : ViewComponent
    {
        private readonly LinkMakerDbContext _context;
        private readonly IDistributedCache _cache;
        public QRCodeList(LinkMakerDbContext context,
            IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }
        //    public async Task<IViewComponentResult> InvokeAsync()
        //    {
        //        string cacheKey = KeyConfigs.QRCode;

        //        // 1. Check Redis
        //        var cachedData = await _cache.GetStringAsync(cacheKey);

        //        if (!string.IsNullOrEmpty(cachedData))
        //        {
        //            var model = JsonSerializer.Deserialize<QRCodeVM[]>(cachedData);
        //            return View(model);
        //        }

        //        // 2. Query database
        //        //var qrCodes = await _context.QRCodes
        //        //    .Select<QRCode, QRCodeVM>(s => new QRCodeVM
        //        //    {
        //        //        Id = s.Id,
        //        //        URL = s.URL,
        //        //        FileName = "~/qrcodes/" + s.FileName,
        //        //    })
        //        //    .ToArrayAsync();
        //        var data = await _context.QRCodesS
        //.AsNoTracking()
        //.ToListAsync(); // Get the raw entities first

        //        var qrCodes = data.Select(s => new QRCodeVM
        //        {
        //            Id = s.Id,
        //            URL = s.URL,
        //            FileName = "/qrcodes/" + s.FileName
        //        }).ToArray();

        //        // 3. Save to Redis
        //        var options = new DistributedCacheEntryOptions()
        //            .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
        //        var json = JsonSerializer.Serialize(qrCodes);
        //        await _cache.SetStringAsync(cacheKey, json, options);

        //        return View(qrCodes); // Trả về View Default.cshtml
        //    }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            string cacheKey = LinkMaker.Common.Contants.KeyConfigs.QRCode;

            // 1. Check Redis
            var cachedData = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                // Use full path for the VM
                var model = JsonSerializer.Deserialize<LinkMaker.MVC.Models.QRCodeVM[]>(cachedData);
                return View(model);
            }

            // 2. Query database - Using full names to bypass the CS0411 error
            var qrCodes = await _context.QRCodes
                .AsNoTracking()
                .Select(s => new LinkMaker.MVC.Models.QRCodeVM
                {
                    Id = s.Id,
                    URL = s.URL,
                    FileName = "/qrcodes/" + s.FileName
                })
                .ToArrayAsync();

            // 3. Save to Redis
            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
            var json = JsonSerializer.Serialize(qrCodes);
            await _cache.SetStringAsync(cacheKey, json, options);

            return View(qrCodes);
        }


    }
}
