using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models
{
    public class Registration
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime DateOfRegistration { get; set; } = DateTime.UtcNow;     
        public DateTime LastLogin { get; set; }     
    }
}
