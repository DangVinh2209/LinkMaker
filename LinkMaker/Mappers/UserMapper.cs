using LinkMaker.Common.DTOs;
using LinkMaker.MVC.Models;

namespace StudentManager.MVC.Mappers
{
    public static class UserMapper
    {
        // DTO -> ViewModel
        public static UserVM ToViewModel(this UserDTO dto)
        {
            if (dto == null) return null;

            return new UserVM
            {
                Id = dto.Id,
                FullName = dto.FullName,
                Email = dto.Email,
                DateOfBirth = dto.DateOfBirth,
                //Gender = dto.Gender,
                Address = dto.Address,
                Phone = dto.Phone,
                //Hobby = dto.Hobby,
                Url = dto.Url,
                UrlId = dto.UrlId,
                //StudentClassId = dto.StudentClassId,
                AvatarPath = dto.AvatarPath
            };
        }

        // ViewModel -> DTO
        public static UserDTO ToDTO(this UserVM vm)
        {
            if (vm == null) return null;

            return new UserDTO
            {
                Id = vm.Id,
                FullName = vm.FullName,
                Email = vm.Email,
                DateOfBirth = vm.DateOfBirth,
                //Gender = vm.Gender,
                Address = vm.Address,
                Phone = vm.Phone,
                //Hobby = vm.Hobby,
                UrlId = vm.UrlId,
                Url = vm.Url,
                //StudentClassId = vm.StudentClassId,
                AvatarPath = vm.AvatarPath
            };
        }
    }
}
