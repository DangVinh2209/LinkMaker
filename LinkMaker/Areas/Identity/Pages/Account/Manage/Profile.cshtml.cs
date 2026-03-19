using LinkMaker.Data.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LinkMaker.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        private readonly UserManager<LinkMakerUser> _userManager;

        public ProfileModel(UserManager<LinkMakerUser> userManager)
        {
            _userManager = userManager;
        }

        public LinkMakerUser CurrentUser { get; set; }

        public async Task OnGetAsync()
        {
            CurrentUser = await _userManager.GetUserAsync(User);
        }
    }
}