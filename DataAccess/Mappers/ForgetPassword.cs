using DataAccess.DtoModels;
using DataAccess.Mappers.Interface;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Mappers
{
    public class ForgetPassword : IForgetPassword
    {
        public Registration Map(ForgetPasswordDTO forgetPasswordDto)
        {
            var registration = new Registration()
            {
                Email = forgetPasswordDto.Email
            };
            return registration;
        }
    }
}
