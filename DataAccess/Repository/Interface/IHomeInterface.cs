using DataAccess.DtoModels.Request;
using DataAccess.DtoModels.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repository.Interface
{
    public interface IHomeRepository
    {
        public string Contact(ContactDTO contact);
        public int CountViews();
        public List<VisitorsResponse> CountViewsTable();
    }
}
