using DataAccess.DtoModels.Request;
using DataAccess.Mappers.Interface;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Mappers
{
    public class LoginMapper : ILoginMapper
    {
        public Registration Map(LoginDTO loginDto)
        {
            var login = new Registration()
            {
                Email = loginDto.Email,
                Password = loginDto.Password
            };
            return login;
        }
    }
}
