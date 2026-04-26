using DataAccess.DtoModels;
using DataAccess.Models;
using ExpensesTracker.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repository.Interface
{
    public interface IMemberRepository
    {
        public string AddMember(RegistrationDTO registrationDto);
        public string Login(LoginDTO loginDto);
        public TokenData GetUserDetails(string token);
        public string GenerateToken(string email, string name, int id);
    }
}
