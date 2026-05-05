using DataAccess.DtoModels.Request;
using DataAccess.DtoModels.Response;
using DataAccess.Helper.Interface;
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
        private readonly IHelper _helper;
        public HomeRepository(DbPostgreContext postgreContext, IContactMapper contactMapper, IHelper helper) 
        { 
            _postgreContext = postgreContext;
            _contactMapper = contactMapper;
            _helper = helper;
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
                dateTime = DateTime.UtcNow
            };
            _postgreContext.countViews.Add(countViews);
            _postgreContext.SaveChanges();

            var totalViews = _postgreContext.countViews.Count();
            return totalViews;

        }

        public List<VisitorsResponse> CountViewsTable()
        {
            var visitorsResponse = new List<VisitorsResponse>();
            var countViewsList = _postgreContext.countViews.OrderByDescending(v => v.dateTime).ToList();
            foreach (var countView in countViewsList)
            {
                visitorsResponse.Add(new VisitorsResponse
                {
                    Id = countView.Id,
                    dateTime = _helper.ConvertUtcToIndiaTime(countView.dateTime)
                });
            }
            return visitorsResponse;
        }
    }
}
