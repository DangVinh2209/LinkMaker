using System;
using System.Collections.Generic;

namespace LinkMaker.Data.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FullName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string? Avatar { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }

        // This is the ONLY link to the Url table. No other properties exist.
        public virtual ICollection<Url> Urls { get; set; } = new List<Url>();
    }
}