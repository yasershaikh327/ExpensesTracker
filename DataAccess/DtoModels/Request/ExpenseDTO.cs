using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static DataAccess.Helper.Helper;

namespace DataAccess.DtoModels.Request
{
    public class ExpenseDTO
    {
        [Required]
        public string ExpenseType { get; set; }

        public int UserId { get; set; } = 0;

        [Required]
        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Amount { get; set; } = decimal.Zero;

        [Required]
        public string? Category { get; set; }
        public string? OtherCatInput { get; set; } = string.Empty;

        [Required]
        public DateOnly? DateofExpense { get; set; }
    }
}
