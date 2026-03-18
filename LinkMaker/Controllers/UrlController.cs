using LinkMaker.Common.DTOS;
using LinkMaker.Data;
using LinkMaker.Data.Entities;
using LinkMaker.Data.Interfaces;
using LinkMaker.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Security.Claims;

namespace LinkMaker.Controllers
{
    [Authorize]
    public class UrlController : Controller
    {
        private readonly LinkMakerDbContext _context;
        private readonly IUrlService _serviceUrl;
        public UrlController(IUrlService serviceUrl, LinkMakerDbContext context)
        {
            _context = context;
            _serviceUrl = serviceUrl;
        }
        // GET: UrlController/Index
        public async Task<ActionResult> Index()
        {
            var urlVMs = await _serviceUrl.GetAll(); // lát sửa chỗ này
            return View(urlVMs);
        }

        // GET: Url/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var url = await _serviceUrl.GetById(id.Value);
            if (url == null)
            {
                return NotFound();
            }
            return View(url);
        }

        // GET: HomeController1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HomeController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,YourLink,NewLink")] UrlDTO urlDTO)
        {
            // 1. Get the logged-in Identity User's ID
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdString == null)
            {
                return Unauthorized(); // Backup check just in case they aren't logged in
            }

            var identityId = Guid.Parse(userIdString);

            // 2. CHECK: Does this user exist in our business 'Users' table yet?
            var userExists = await _context.Users.AnyAsync(u => u.Id == identityId);

            // 3. CREATE ON THE FLY: If they don't exist, create their profile using Identity data
            if (!userExists)
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email) ?? User.Identity?.Name ?? "Unknown";

                var newBusinessProfile = new User
                {
                    Id = identityId, // VERY IMPORTANT: Use the exact same ID as Identity
                    Email = userEmail,
                    FullName = userEmail, // Defaulting to email since FullName is required
                    DateOfBirth = DateTime.UtcNow // Defaulting to today since DOB is required
                };

                _context.Users.Add(newBusinessProfile);
                await _context.SaveChangesAsync(); // Save the new user to the database
            }

            // 4. Attach the ID to the DTO and send it to the Service
            if (ModelState.IsValid)
            {
                urlDTO.UserId = identityId;

                var isOK = await _serviceUrl.Create(urlDTO);
                if (isOK)
                {
                    return RedirectToAction(nameof(Create));
                }
            }

            return View(urlDTO);
        }
        public async Task<IActionResult> Edit(Guid id)
        {
            //var major = await _context.Majors.FindAsync(id);
            var majorDTO = await _serviceUrl.GetById(id);
            if (majorDTO == null)
            {
                return NotFound();
            }
            //return View(major);
            return View(nameof(Create), majorDTO);
        }

        // POST: Majors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,YourLink,NewLink,MajorCode")] UrlDTO urlDTO)
        {
            if (id != urlDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(major);
                    //var major = _context.Majors.FindAsync(majorDTO.Id);
                    var isOK = await _serviceUrl.Update(urlDTO);
                    if (isOK)
                    {
                        return RedirectToAction(nameof(Create));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await UrlExists(urlDTO.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(nameof(Create), urlDTO);
        }

        // GET: HomeController1/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // GET: Majors/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var url = await _serviceUrl.GetById(id.Value);

            if (url == null)
            {
                return NotFound();
            }
            var urlVM = new UrlVM(url);

            return PartialView(url);
        }

        // POST: Majors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            bool isOK = false;
            string message = string.Empty;
            try
            {
                //if (await _userManager.IsInRoleAsync(user, "Admin"))
                //{
                //    // logic
                //}

                isOK = await _serviceUrl.Delete(id);
                if (isOK)
                {
                    message = "Xoa thanh cong";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                //message = "Loi thuc thi " + ex.Message;
                //return RedirectToAction(nameof(Index));
                throw;
            }

            //return Json(new { isOK, message });

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Reload()
        {
            return ViewComponent("Url");
        }

        private async Task<bool> UrlExists(Guid id)
        {
            //return _context.Majors.Any(e => e.Id == id);
            var urlDTO = await _serviceUrl.GetById(id);
            return urlDTO != null;
        }
    }
}
