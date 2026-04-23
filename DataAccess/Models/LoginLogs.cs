using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models
{
    public class LoginLogs
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime LoginTime { get; set; } = DateTime.UtcNow;
    }
}
