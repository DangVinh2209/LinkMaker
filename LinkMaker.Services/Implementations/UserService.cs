using LinkMaker.Common.DTOs;
using LinkMaker.Data;
using LinkMaker.Data.Entities;
using LinkMaker.Data.Entities.Identity;
using LinkMaker.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LinkMaker.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IDistributedCache _cache;
        private readonly LinkMakerDbContext _context;
        private const string AllUsersCacheKey = "AllUsers_List";

        public UserService(LinkMakerDbContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        #region Cache Helpers
        private async Task SetCacheAsync<T>(string key, T value)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                SlidingExpiration = TimeSpan.FromMinutes(2)
            };
            var serializedData = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, serializedData, options);
        }

        private async Task ClearUserCacheAsync(Guid? id = null)
        {
            // Always clear the list cache
            await _cache.RemoveAsync(AllUsersCacheKey);

            // If a specific ID is provided, clear that specific user's cache
            if (id.HasValue)
            {
                await _cache.RemoveAsync($"User_{id.Value}");
            }
        }
        #endregion

        public async Task<bool> Create(UserDTO dtoUser)
        {
            try
            {
                var newUser = new User
                {
                    FullName = dtoUser.FullName.Trim(),
                    DateOfBirth = dtoUser.DateOfBirth,
                    Address = dtoUser.Address?.Trim(),
                    Email = dtoUser.Email?.Trim(),
                    Phone = dtoUser.Phone?.Trim(),
                    
                };

                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();

                // Invalidate the list because a new user exists
                await ClearUserCacheAsync();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("DATABASE ERROR: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> Update(UserDTO studentDTO)
        {
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
                   

                    await _context.SaveChangesAsync();

                    // Clear specific user and the list
                    await ClearUserCacheAsync(studentDTO.Id);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Delete(Guid idUser)
        {
            try
            {
                var user = await _context.Users.FindAsync(idUser);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();

                    // Clear specific user and the list
                    await ClearUserCacheAsync(idUser);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<UserDTO[]?> GetAll()
        {
            try
            {
                var cachedData = await _cache.GetStringAsync(AllUsersCacheKey);
                if (!string.IsNullOrEmpty(cachedData))
                {
                    return JsonSerializer.Deserialize<UserDTO[]>(cachedData);
                }

                var users = await _context.Users
                    .Include(s => s.Urls)
                    .Select(s => new UserDTO
                    {
                        Id = s.Id,
                        Address = s.Address,
                        DateOfBirth = s.DateOfBirth,
                        Email = s.Email,
                        FullName = s.FullName,
                        Phone = s.Phone,
                        //Url = s.Urls.YourLink,
                    })
                    .ToArrayAsync();

                if (users != null)
                {
                    await SetCacheAsync(AllUsersCacheKey, users);
                }

                return users;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<UserDTO?> GetById(Guid idUser)
        {
            try
            {
                var cacheKey = $"User_{idUser}";
                var cachedData = await _cache.GetStringAsync(cacheKey);

                if (!string.IsNullOrEmpty(cachedData))
                {
                    return JsonSerializer.Deserialize<UserDTO>(cachedData);
                }

                var user = await _context.Users
                    .Where(s => s.Id == idUser)
                    .Select(s => new UserDTO
                    {
                        Id = s.Id,
                        Address = s.Address,
                        DateOfBirth = s.DateOfBirth,
                        Email = s.Email,
                        FullName = s.FullName,
                        Phone = s.Phone,
                    })
                    .SingleOrDefaultAsync();

                if (user != null)
                {
                    await SetCacheAsync(cacheKey, user);
                }

                return user;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<UserDTO?> GetByIdWithMajor(Guid idUser)
        {
            // Caching this specifically depends on how often 'Major' details change.
            // For now, simple DB fetch:
            try
            {
                return await _context.Users
                    .Where(s => s.Id == idUser)
                    .Include(s => s.Urls)
                    .Select(s => new UserDTO
                    {
                        Id = s.Id,
                        Address = s.Address,
                        DateOfBirth = s.DateOfBirth,
                        Email = s.Email,
                        FullName = s.FullName,
                        Phone = s.Phone,
                        //Url = s.Url.YourLink,
                    })
                    .SingleOrDefaultAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Task<UserDTO?> GetByIdWithLink(Guid idStudent)
        {
            throw new NotImplementedException();
        }
    }
}