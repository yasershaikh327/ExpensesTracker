using DataAccess.DtoModels.Request;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Mappers.Interface
{
    public interface IContactMapper
    {
         Contact Map(ContactDTO contactDto);
    }
}
