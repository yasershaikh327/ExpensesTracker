using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Helper.Interface;
using Microsoft.AspNetCore.Identity;

namespace DataAccess.Helper
{
    public class Helper : IHelper
    {
        public string HashPassword(string password)
        {
            var passwordHasher = new PasswordHasher<object>();

            return passwordHasher.HashPassword(null, password);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            var passwordHasher = new PasswordHasher<object>();

            var result = passwordHasher.VerifyHashedPassword(
                null,
                hashedPassword,
                password
            );

            return result == PasswordVerificationResult.Success;
        }
    }
}
