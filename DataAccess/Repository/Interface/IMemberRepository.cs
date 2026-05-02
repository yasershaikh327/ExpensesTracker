using DataAccess.DtoModels.Request;
using DataAccess.DtoModels.Response;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repository.Interface
{
    public interface IMemberRepository
    {
        public bool AddMember(RegistrationDTO registrationDto);
        public string Login(LoginDTO loginDto);
        public string AddExpense(ExpenseDTO expense);
        public TokenData GetUserDetailsByToken(string token);
        public RegistrationDTO GetUserDetailsByEmail(string Email);
        public string GenerateToken(string email, string name, int id);
        //public string ForgetPassword(ForgetPasswordDTO forgetPassword);
        public bool CheckIfEmailExists(ForgetPasswordDTO forgetPassword);
        public int ResetPassword(ResetPasswordDTO resetPassword);
        public DashboardResponse GetDashboardData(int userId);
    }
}
