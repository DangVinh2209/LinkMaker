using Microsoft.AspNetCore.Identity;

namespace LinkMaker.Data.Entities.Identity
{
    public class UserManagerRole : IdentityRole
    {
        public string? Description { get; set; }
    }
}
