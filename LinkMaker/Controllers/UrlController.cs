using LinkMaker.Data;
using LinkMaker.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LinkMaker.Controllers
{
    public class UrlController : Controller
    {
        private readonly LinkMakerDbContext _context;
        public UrlController(LinkMakerDbContext context)
        {
            _context = context;
        }
        // GET: UrlController/Index
        public ActionResult Index()
        {
            return View(_context.Urls.ToList());
        }

        // GET: HomeController1/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HomeController1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HomeController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Url link)
        {
            if (link != null)
            {
                _context.Urls.Add(link);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(link);
        }

        // GET: HomeController1/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HomeController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HomeController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public ActionResult AccessLink()
        {
            return View(_context.Urls.ToList());
        }
        public IActionResult Go(string code)
        {
            var link = _context.Urls.FirstOrDefault(l => l.NewLink == code);

            if (link == null)
            {
                return NotFound();
            }

            return Redirect(link.YourLink);
        }
    }
}
