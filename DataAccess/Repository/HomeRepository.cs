using DataAccess.DtoModels;
using DataAccess.Mappers.Interface;
using DataAccess.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repository
{
    public class HomeRepository : IHomeRepository
    {
        private readonly DbPostgreContext _postgreContext;
        private readonly IContactMapper _contactMapper;
        public HomeRepository(DbPostgreContext postgreContext, IContactMapper contactMapper) 
        { 
            _postgreContext = postgreContext;
            _contactMapper = contactMapper;
        }
        public string Contact(ContactDTO contactDto)
        {
            var mapper = _contactMapper.Map(contactDto);
            _postgreContext.Add(mapper);
            _postgreContext.SaveChanges();
            return "Contact form submitted successfully.";
        }
    }
}
