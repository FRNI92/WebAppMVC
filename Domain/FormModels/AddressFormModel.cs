using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.FormModels
{
    public class AddressFormModel
    {

        public int Id { get; set; }  // adding this since I cant update mmember
        public string? StreetName { get; set; }
        public string? StreetNumber { get; set; }
        public string? PostalCode { get; set; }
        public string? City { get; set; }
    }
}
