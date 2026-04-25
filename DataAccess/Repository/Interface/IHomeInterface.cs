using DataAccess.DtoModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repository.Interface
{
    public interface IHomeRepository
    {
        public string Contact(ContactDTO contact);
    }
}
