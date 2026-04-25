using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public int? Rating { get; set; }
        public string? Subject { get; set; }
        public string? Message { get; set; }
        public DateTime? Created { get; set; } = DateTime.UtcNow;
    }
}
