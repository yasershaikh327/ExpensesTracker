using DataAccess.DtoModels.Request;
using DataAccess.Mappers.Interface;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Mappers
{
    public class ExpenseMapper : IExpenseMapper
    {
        public Expense Map(ExpenseDTO expenseDto)
        {
           var expense = new Expense()
           {
                ExpenseType = expenseDto.ExpenseType,
                Description = expenseDto.Description,
                Amount = expenseDto.Amount ?? 0.0m,
                Category = expenseDto.Category,
                UserId = expenseDto.UserId,
                DateofExpense = expenseDto.DateofExpense
    
           };
            return expense;
        }
    }
}   