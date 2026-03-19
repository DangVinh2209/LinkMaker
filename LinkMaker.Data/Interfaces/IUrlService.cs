using LinkMaker.Common.DTOS;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinkMaker.Data.Interfaces
{
    public interface IUrlService
    {
        Task<bool> Create(UrlDTO dtoUrl);

        Task<UrlDTO[]?> GetAll();
        Task<UrlDTO?> GetById(Guid idUrl);
        Task<bool> Update(UrlDTO dtoUrl);
        Task<bool> Delete(Guid idUrl);
        Task<UrlDTO[]?> GetByUserId(Guid userId);
        Task<UrlDTO?> GetById(Guid idUrl, Guid userId);

    }
}
