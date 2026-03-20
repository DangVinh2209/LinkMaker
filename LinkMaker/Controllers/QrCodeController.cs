using LinkMaker.Common.Contants;
using LinkMaker.Data;
using LinkMaker.Models;
using LinkMaker.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;

namespace LinkMaker.MVC.Controllers
{
    [EnableRateLimiting(Configs.IPPolicyLimiter)]
    public class QRCodeController : Controller
    {
        private readonly LinkMakerDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IDistributedCache _cache;


        public QRCodeController(LinkMakerDbContext context,
            IWebHostEnvironment env,
            IDistributedCache cache)
        {
            _context = context;
            _env = env;
            _cache = cache;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QRCodeVM model)
        {
            //TODO: === Code xu73 ly backend tai day ===//
            if (string.IsNullOrEmpty(model.URL))
            {
                return BadRequest();
            }
            string fileName = "";
            string filePath = "";

            try
            {
                using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                {
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(model.URL, QRCodeGenerator.ECCLevel.Q);

                    using (QRCode qrCode = new QRCode(qrCodeData))
                    {
                        using (Bitmap qrBitmap = qrCode.GetGraphic(20))
                        {
                            // đường dẫn wwwroot
                            string wwwrootPath = _env.WebRootPath;

                            // thư mục lưu QR
                            string folderPath = Path.Combine(wwwrootPath, "qrcodes");

                            if (!Directory.Exists(folderPath))
                            {
                                Directory.CreateDirectory(folderPath);
                            }

                            // tên file
                            fileName = $"{Guid.NewGuid()}.png";

                            filePath = Path.Combine(folderPath, fileName);

                            // lưu file
                            qrBitmap.Save(filePath, ImageFormat.Png);


                            //string fileUrl = $"/qrcodes/{fileName}";

                            //return Content($"QR Code saved: {fileUrl}");



                            //using (MemoryStream ms = new MemoryStream())
                            //{
                            //    qrBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                            //    return File(ms.ToArray(), "image/png");
                            //}
                        }
                    }

                }
                var newQRCode = new LinkMaker.Data.Entities.QRCode
                {
                    URL = model.URL.Trim(),
                    FileName = fileName,
                };
                await _context.AddAsync(newQRCode);
                await _context.SaveChangesAsync();

                //TODO: Delete cache
                await _cache.RemoveAsync(KeyConfigs.QRCode);

                // Tra ve đường dẫn hiển thị trên web
                return RedirectToAction(nameof(Create));
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }

            return View();
        }



    }
}
