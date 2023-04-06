using LBT_Api.Entities;
using LBT_Api.Models.AddressDto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBT_Api.Tests
{
    public static class Tools
    {
        // General
        public static string AsJSON(object obj)
        {
            string output = JsonConvert.SerializeObject(obj);
            return output;
        }

        public static void AssertObjectsAreSameAsJSON(object obj1, object obj2)
        {
            Assert.That(AsJSON(obj1), Is.EqualTo(AsJSON(obj2)));
        }

        // Address
        public static CreateAddressDto GetExampleCreateAddressDto()
        {
            CreateAddressDto dto = new CreateAddressDto()
            {
                Country = "Country",
                City = "City",
                Street = "Street",
                BuildingNumber = "BuildingNumber",
            };

            return dto;
        }

        public static Address GetExampleAddress()
        {
            Address address = new Address()
            {
                Country = "Country",
                City = "City",
                Street = "Street",
                BuildingNumber = "BuildingNumber",
            };

            return address;
        }
    }
}
