//using LinkMaker.Common.Contants;
//using LinkMaker.Common.Enums;
using LinkMaker.Common.Contants;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LinkMaker.Common.DTOs
{
    public class UserDTO
    {
        public Guid Id { get; set; }

        [MaxLength(MaxLengths.FULL_NAME)]
        public string FullName { get; set; } = string.Empty;

        //public GenderEnum Gender { get; set; }
        //public HobbyEnum Hobby { get; set; }
        public DateTime DateOfBirth { get; set; }

        //[Description("Duong dan hinh anh dai dien")]
        ////[MaxLength(MaxLengths.AVATAR)]
        //public IFormFile? Avatar { get; set; }
        public string? AvatarPath { get; set; }

        [MaxLength(15)]
        public string? Phone { get; set; }
        [MaxLength(250)]
        public string? Email { get; set; }
        [MaxLength(500)]
        public string? Address { get; set; }

        public Guid? UrlId { get; set; }
        public string? Url { get; set; }

        //public Guid StudentClassId { get; set; }
        //public string? StudentClass { get; set; }

    }
}
