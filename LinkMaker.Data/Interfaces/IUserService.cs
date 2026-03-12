using System;
using System.Collections.Generic;
using System.Text;
using LinkMaker.Common.DTOs;
using LinkMaker.Data.Entities;

namespace LinkMaker.Data.Interfaces
{
    public interface IUserService
    {
        Task<bool> Create(UserDTO userDTO);

        Task<UserDTO?> GetById(Guid idUser);
        Task<UserDTO[]?> GetAll();
        Task<UserDTO?> GetByIdWithLink(Guid idUser);
        Task<bool> Update(UserDTO userDTO);
        Task<bool> Delete(Guid idUser);

    }
}
