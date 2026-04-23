using DataAccess.Models;
using ExpensesTracker.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Mappers.Interface
{
    public interface IRegistrationMapper
    {
        Registration Map(RegistrationDTO registrationDTO);
    }
}
