using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.DtoModels.Response
{
    public class DashboardResponse
    {
        public decimal TotalSpent { get; set; } = 0;
        public decimal TotalIncome { get; set; } = 0;
        public decimal NetSaving { get; set; } = 0;
        public int NoofTransactions { get; set; } = 0;
        public int Credits { get; set; } = 0;
        public char Status { get; set; } = 'N';
        public List<TransactionResponseDto> Transaction { get; set; } = new List<TransactionResponseDto>();
    }

    public class TransactionResponseDto
    {
        public DateOnly? Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public decimal Amount { get; set; } = decimal.Zero;
        public char Type { get; set; } = 'D';
        public string? Category { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string? Type2 { get; set; } = string.Empty;
    }
}
