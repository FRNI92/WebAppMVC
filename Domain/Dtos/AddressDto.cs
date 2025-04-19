using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class AddressDto
    {
        public string? StreetName { get; set; }
        public string? StreetNumber { get; set; }
        public string? PostalCode { get; set; }
        public string? City { get; set; }
    }
}
