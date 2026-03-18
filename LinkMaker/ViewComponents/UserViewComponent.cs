using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LinkMaker.Common.DTOs;
using LinkMaker.Data;

namespace StudentManager.MVC.ViewComponents
{
    public class UsersViewComponent : ViewComponent
    {
        private readonly LinkMakerDbContext _context;

        public UsersViewComponent(LinkMakerDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var users = await _context.Users
                .Include(s => s.Urls)
                .Select(s => new UserDTO
                {
                    Id = s.Id,
                    FullName = s.FullName,
                    Phone = s.Phone,
                    Email = s.Email,
                    Address = s.Address,
                    DateOfBirth = s.DateOfBirth,
                    //Gender = s.Gender,
                    //Url = s.Urls.YourLink,
                })
                .ToListAsync();
            return View(users); // Trả về View Default.cshtml
        }
    }
}
