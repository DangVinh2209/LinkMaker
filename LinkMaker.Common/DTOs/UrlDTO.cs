using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LinkMaker.Common.DTOS
{
    public class UrlDTO
    {

        public Guid Id { get; set; } = Guid.NewGuid();
        public string YourLink { get; set; } = string.Empty;
        public string NewLink { get; set; } = string.Empty;
        public string? UrlCode { get; set; }

    }
}
