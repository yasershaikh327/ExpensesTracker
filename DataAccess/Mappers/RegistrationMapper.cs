using DataAccess.DtoModels.Request;
using DataAccess.Mappers.Interface;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Mappers
{
    public class RegistrationMapper : IRegistrationMapper
    {
        public Registration Map(RegistrationDTO registrationDTO)
        {
            var registration = new Registration()
            {
                Name = registrationDTO.Name,
                Email = registrationDTO.Email,
                Password = registrationDTO.Password
            };
           
            return registration;
        }
    }
}
