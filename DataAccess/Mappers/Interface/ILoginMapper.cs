using DataAccess.DtoModels;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Mappers.Interface
{
    public interface ILoginMapper
    {
        Registration Map(LoginDTO loginDto);    
    }
}
