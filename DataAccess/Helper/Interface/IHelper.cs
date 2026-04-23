using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Helper.Interface
{
    public interface IHelper
    {
        public string HashPassword(string password);
        public bool VerifyPassword(string password, string hashedPassword);
    }
}
