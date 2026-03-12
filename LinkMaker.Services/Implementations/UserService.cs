using LinkMaker.Common.DTOs;
using LinkMaker.Data;
using LinkMaker.Data.Entities;
using LinkMaker.Data.Entities.Identity;
using LinkMaker.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkMaker.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly LinkMakerDbContext _context;
        public UserService(LinkMakerDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Create(UserDTO dtoUser)
        {
            //throw new NotImplementedException();
            var isOK = false;
            try
            {
                var newUser = new User
                {
                    FullName = dtoUser.FullName.Trim(),
                    DateOfBirth = dtoUser.DateOfBirth,
                    Address = dtoUser.Address?.Trim(),
                    Email = dtoUser.Email?.Trim(),
                    Phone = dtoUser.Phone?.Trim(),
                    //Gender = (int)dtoStudent.Gender,
                    //Gender = dtoUser.Gender,
                    //Hobby = dtoUser.Hobby,
                    UrlId = dtoUser.UrlId,
                    //StudentClassId = dtoStudent.StudentClassId,
                    Avatar = dtoUser.AvatarPath,
                };
                await _context.Users.AddAsync(newUser);

                //if (dtoStudent.Avatar != null && dtoStudent.Avatar.Length > 0)
                //{
                //    var file = dtoStudent.Avatar;
                //    var extension = Path.GetExtension(file.FileName).ToLower();
                //    var arrayAllowedFile = new string[] { ".jpg", ".jpeg", ".png" };
                //    if (arrayAllowedFile.Contains(extension))
                //    {
                //        var allowedSize = 5 * 1024 * 1024;
                //        if (file.Length < allowedSize)
                //        {
                //            var fileName = Guid.NewGuid().ToString() + extension;
                //            var wwwRootFolder = _environment.WebRootPath;
                //            var mediaFolder = Path.Combine(wwwRootFolder, "media", "images");
                //            if (!Directory.Exists(mediaFolder))
                //            {
                //                Directory.CreateDirectory(mediaFolder);
                //            }
                //            var destinationFile = Path.Combine(mediaFolder, fileName);
                //            using (var fileStream = new FileStream(destinationFile, FileMode.Create))
                //            {
                //                await file.CopyToAsync(fileStream);
                //            }
                //            newStudent.Avatar = fileName;
                //        }
                //    }
                //}
                await _context.SaveChangesAsync();
                isOK = true;
            }
            catch (Exception ex)
            {
                // Add this to see the actual error message
                System.Diagnostics.Debug.WriteLine("DATABASE ERROR: " + ex.Message);
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine("INNER ERROR: " + ex.InnerException.Message);
                }

            }
            return isOK;
        }

        public async Task<bool> Delete(Guid idUser)
        {
            var isOK = false;
            try
            {
                var user = await _context.Users.FindAsync(idUser);
                if (user != null)
                {
                    _context.Users.Remove(user);
                }

                await _context.SaveChangesAsync();
                isOK = true;
            }
            catch (Exception ex)
            {

            }

            return isOK;
        }

        public async Task<UserDTO[]?> GetAll()
        {
            //throw new NotImplementedException();
            try
            {
                var user = await _context.Users
                    .Include(s => s.Url)
                    .Select(s => new UserDTO
                    {
                        Id = s.Id,
                        Address = s.Address,
                        //AvatarPath = s.Avatar,
                        DateOfBirth = s.DateOfBirth,
                        Email = s.Email,
                        FullName = s.FullName,
                        Phone = s.Phone,
                        //Gender = s.Gender,
                        //Hobby = s.Hobby,
                        UrlId = s.UrlId,
                        Url = s.Url.YourLink,
                        //Url = s.Url.NewLink,
                        //StudentClassId = s.StudentClassId,
                    })
                    .ToArrayAsync();
                return user;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<UserDTO?> GetById(Guid idUser)
        {
            //throw new NotImplementedException();
            try
            {
                var user = await _context.Users
                    .Where(s => s.Id == idUser)
                    .Select(s => new UserDTO
                    {
                        Id = s.Id,
                        Address = s.Address,
                        //AvatarPath = s.Avatar,
                        DateOfBirth = s.DateOfBirth,
                        Email = s.Email,
                        FullName = s.FullName,
                        Phone = s.Phone,
                        //Gender = s.Gender,
                        //Hobby = s.Hobby,
                        UrlId = s.UrlId,
                        //StudentClassId = s.StudentClassId,
                    })
                    .SingleOrDefaultAsync();
                return user;
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public Task<UserDTO?> GetByIdWithLink(Guid idStudent)
        {
            throw new NotImplementedException();
        }

        public async Task<UserDTO?> GetByIdWithMajor(Guid idUser)
        {
            //throw new NotImplementedException();
            try
            {
                var user = await _context.Users
                    .Where(s => s.Id == idUser)
                    .Include(s => s.Url)
                    .Select(s => new UserDTO
                    {
                        Id = s.Id,
                        Address = s.Address,
                        //AvatarPath = s.Avatar,
                        DateOfBirth = s.DateOfBirth,
                        Email = s.Email,
                        FullName = s.FullName,
                        Phone = s.Phone,
                        //Gender = s.Gender,
                        //Hobby = s.Hobby,
                        UrlId = s.UrlId,
                        Url = s.Url.YourLink,
                        //StudentClassId = s.StudentClassId,
                    })
                    .SingleOrDefaultAsync();
                return user;
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public async Task<bool> Update(UserDTO studentDTO)
        {
            //throw new NotImplementedException();
            var isOK = false;
            try
            {
                var user = await _context.Users.FindAsync(studentDTO.Id);
                if (user != null)
                {
                    user.FullName = studentDTO.FullName.Trim();
                    user.Address = studentDTO.Address?.Trim();
                    user.Phone = studentDTO.Phone?.Trim();
                    user.Email = studentDTO.Email?.Trim();
                    user.DateOfBirth = studentDTO.DateOfBirth;
                    //user.Gender = studentDTO.Gender;
                    //user.Hobby = studentDTO.Hobby;
                    user.UrlId = studentDTO.UrlId;
                    //user.StudentClassId = studentDTO.StudentClassId;

                    //_context.Update(user);
                    await _context.SaveChangesAsync();
                    isOK = true;
                }
            }
            catch (Exception ex)
            {

            }
            return isOK;
        }



    }
}
