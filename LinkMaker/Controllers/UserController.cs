using LinkMaker.Common.DTOs;
using LinkMaker.Data;
using LinkMaker.Data.Entities;
using LinkMaker.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LinkMaker.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly LinkMakerDbContext _context;

        public UserController(LinkMakerDbContext context, IUserService userService)
        {   
            _userService = userService;
            _context = context;
        }

        // GET: UserController1
        public async Task<IActionResult> Index()
        {
            var linkMakerDbContext = _context.Users.Include(u => u.Urls);
            return View(await linkMakerDbContext.ToListAsync());
        }

        // GET: UserController1/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Urls)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: UserController1/Create
        public IActionResult Create()
        {
            ViewData["UrlId"] = new SelectList(_context.Urls, "Id", "NewLink");
            return View();
        }

        // POST: UserController1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,FullName,DateOfBirth,Avatar,Phone,Email,Address,UrlId")] User user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        user.Id = Guid.NewGuid();
        //        _context.Add(user);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["UrlId"] = new SelectList(_context.Urls, "Id", "NewLink", user.UrlId);
        //    return View(user);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserDTO userDto) // Use the DTO here
        {

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    System.Diagnostics.Debug.WriteLine($"VALIDATION ERROR: {error.ErrorMessage}");
                }
            }
            // Check if the model is valid
            if (ModelState.IsValid)
            {


                var success = await _userService.Create(userDto);   
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            // If we got here, something failed.
            ViewData["UrlId"] = new SelectList(_context.Urls, "Id", "NewLink", userDto.UrlId);
            return View(userDto);
        }

        // GET: UserController1/Edit/5
        //public async Task<IActionResult> Edit(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var user = await _context.Users.FindAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["UrlId"] = new SelectList(_context.Urls, "Id", "NewLink", user.UrlId);
        //    return View(user);
        //}

        // POST: UserController1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,FullName,DateOfBirth,Avatar,Phone,Email,Address")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            
            return View(user);
        }

        // GET: UserController1/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Urls)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: UserController1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
