using Microsoft.AspNetCore.Mvc;
//using LinkMaker.Common.Contants;
using LinkMaker.Common.DTOs;
//using LinkMaker.Common.Enums;
using LinkMaker.Data.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using LinkMaker.Common.Contants;

namespace LinkMaker.MVC.Models
{
    [Bind("Id,FullName,Avatar,Gender,Hobby, DateOfBirth,Phone,Email,Address,MajorId, StudentClassId")]
    public class UserVM
    {
        public UserVM()
        {

        }
        public UserVM(UserDTO user)
        {
            Id = user.Id;
            Url = user.Url;
            Address = user.Address;
            FullName = user.FullName;
            DateOfBirth = user.DateOfBirth;
            Email = user.Email;
            //Gender = user.Gender;
            //Hobby = user.Hobby;
            UrlId = user.UrlId;
            Phone = user.Phone;
            //StudentClassId = user.StudentClassId;
        }
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(MaxLengths.FULL_NAME)]
        public string FullName { get; set; } = string.Empty;

        //public int Gender { get; set; }
        //public GenderEnum Gender { get; set; }
        //public HobbyEnum Hobby { get; set; }

        public DateTime DateOfBirth { get; set; }

        [Description("Duong dan hinh anh dai dien")]
        //[MaxLength(MaxLengths.AVATAR)]
        public IFormFile? Avatar { get; set; }
        public string? AvatarPath { get; set; }

        [MaxLength(MaxLengths.PHONE)]
        public string? Phone { get; set; }

        [MaxLength(MaxLengths.EMAIL)]
        public string? Email { get; set; }
        [MaxLength(MaxLengths.ADDRESS)]
        public string? Address { get; set; }

        public Guid? UrlId { get; set; }
        public string? Url { get; set; }

        //public Guid StudentClassId { get; set; }
        //public string? StudentClass { get; set; }
    }
}
