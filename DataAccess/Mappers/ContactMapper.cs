using DataAccess.DtoModels;
using DataAccess.Mappers.Interface;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Mappers
{
    public class ContactMapper : IContactMapper
    {
        public Contact Map(ContactDTO contactDto)
        {
            var contact = new Contact()
            {
                Name = contactDto.Name,
                Email = contactDto.Email,
                Subject = contactDto.Subject,
                Rating = contactDto.Rating,
                Message = contactDto.Message,
                Created = DateTime.UtcNow
            };
            
            return contact;
        }
    }
}
