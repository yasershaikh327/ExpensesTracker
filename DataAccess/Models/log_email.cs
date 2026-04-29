using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models
{
    public class log_email
    {
        public int Id { get; set; }
        public string SenderEmail { get; set; }
        public string senderName { get; set; }
        public string recipientEmail { get; set; }
        public string recipientName { get; set; }
        public string Subject { get; set; }
        public string htmlContent { get; set; }
        public DateTime createdAt { get; set; } = DateTime.UtcNow;
    }
}
