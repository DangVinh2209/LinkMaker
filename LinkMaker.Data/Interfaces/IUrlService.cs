using LinkMaker.Common.DTOS;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinkMaker.Data.Interfaces
{
    public interface IUrlService
    {
        Task<bool> Create(UrlDTO dtoUrl);
    }
}
