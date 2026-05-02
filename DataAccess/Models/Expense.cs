using System;
using System.Collections.Generic;
using System.Text;
using static DataAccess.Helper.Helper;

namespace DataAccess.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public string ExpenseType { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public string? Category { get; set; }
        public string? OtherCatInput { get; set; } = null;
        public DateOnly? DateofExpense { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public int UserId { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
