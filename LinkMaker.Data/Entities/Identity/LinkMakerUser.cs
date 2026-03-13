using Microsoft.AspNetCore.Identity;

namespace LinkMaker.Data.Entities.Identity
{
    public class LinkMakerUser : IdentityUser
    {
        public string? Avatar { get; set; }
        public string? FullName { get; set; }
    }
}
