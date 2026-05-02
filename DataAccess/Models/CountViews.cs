using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models
{
    public class CountViews
    {
        public int Id { get; set; }
        public DateTime dateTime { get; set; } = DateTime.UtcNow;
    }
}
