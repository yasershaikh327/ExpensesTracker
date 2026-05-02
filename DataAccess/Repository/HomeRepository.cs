using DataAccess.DtoModels.Request;
using DataAccess.Mappers.Interface;
using DataAccess.Models;
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

        public int CountViews()
        {
            var countViews = new CountViews()
            {
                dateTime = DateTime.Now
            };
            _postgreContext.countViews.Add(countViews);
            _postgreContext.SaveChanges();

            var totalViews = _postgreContext.countViews.Count();
            return totalViews;

        }
    }
}
