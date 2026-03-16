//using LinkMaker.Common.Enums;
using System.ComponentModel;

namespace LinkMaker.Data.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        //public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        //public int Gender { get; set; }
        //public GenderEnum Gender { get; set; }
        //public HobbyEnum Hobby { get; set; }
        public DateTime DateOfBirth { get; set; }

        [Description("Duong dan hinh anh dai dien")]
        public string? Avatar { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public virtual Url? Url { get; set; } = null!;    
        public virtual Url? NewLink { get; set; } = null!;
        public Guid? UrlId { get; set; }
        //public virtual Major Major { get; set; } = null!;

        //public Guid StudentClassId { get; set; }
        //public virtual StudentClass StudentClass { get; set; } = null!;

    }
}
