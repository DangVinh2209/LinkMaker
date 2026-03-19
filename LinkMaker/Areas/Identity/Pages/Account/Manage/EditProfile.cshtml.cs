#nullable disable

using System.ComponentModel.DataAnnotations;
using LinkMaker.Data.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LinkMaker.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class EditProfileModel : PageModel
    {
        private readonly UserManager<LinkMakerUser> _userManager;
        private readonly SignInManager<LinkMakerUser> _signInManager;

        public EditProfileModel(
            UserManager<LinkMakerUser> userManager,
            SignInManager<LinkMakerUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Full Name")]
            public string FullName { get; set; }

            [Phone]
            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            //[Display(Name = "Avatar URL")]
            //public string Avatar { get; set; }

            //[Display(Name = "Description")]
            //public string Description { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Current Password")]
            public string CurrentPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "New Password")]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm New Password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmNewPassword { get; set; }
        }

        private async Task LoadAsync(LinkMakerUser user)
        {
            Input = new InputModel
            {
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                //Avatar = user.Avatar,
                //Description = user.Description
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("Unable to load user.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("Unable to load user.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            user.FullName = Input.FullName;
            user.PhoneNumber = Input.PhoneNumber;
            //user.Avatar = Input.Avatar;
            //user.Description = Input.Description;

            if (user.Email != Input.Email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
                if (!setEmailResult.Succeeded)
                {
                    foreach (var error in setEmailResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return Page();
                }

                var setUserNameResult = await _userManager.SetUserNameAsync(user, Input.Email);
                if (!setUserNameResult.Succeeded)
                {
                    foreach (var error in setUserNameResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return Page();
                }
            }

            if (!string.IsNullOrWhiteSpace(Input.NewPassword))
            {
                if (string.IsNullOrWhiteSpace(Input.CurrentPassword))
                {
                    ModelState.AddModelError(string.Empty, "You must enter your current password to set a new password.");
                    return Page();
                }

                var changePasswordResult = await _userManager.ChangePasswordAsync(
                    user,
                    Input.CurrentPassword,
                    Input.NewPassword
                );

                if (!changePasswordResult.Succeeded)
                {
                    foreach (var error in changePasswordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return Page();
                }
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Profile updated successfully.";
            return RedirectToPage();
        }
    }
}