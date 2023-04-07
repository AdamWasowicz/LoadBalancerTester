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

        // Company
        public static Company GetExampleCompany(int addressId = -1, int contactInfoId = -1)
        {
            Company company = new Company()
            {
                AddressId = addressId,
                ContactInfoId = contactInfoId,
                Name = "Name",
            };

            return company;
        }

        /// <summary>
        /// Creates dependent rows of Company and returns Company
        /// </summary>
        /// <param name="dbContext">Context for Db</param>
        /// <returns>Company that was not yet added to dbContext</returns>
        public static Company GetExampleCompanyWithDependecies(LBT_DbContext dbContext)
        {
            // Address
            Address address = GetExampleAddress();
            dbContext.Addresses.Add(address);
            dbContext.SaveChanges();

            // ContactInfo
            ContactInfo contactInfo = GetExampleContactInfo();
            dbContext.ContactInfos.Add(contactInfo);
            dbContext.SaveChanges();

            // Company
            Company company = GetExampleCompany(address.Id, contactInfo.Id);

            return company;
        }

        // ContactInfo
        public static ContactInfo GetExampleContactInfo()
        {
            ContactInfo contactInfo = new ContactInfo()
            {
                Email = "Email",
                PhoneNumber = "PhoneNumber",
            };

            return contactInfo;
        }

    }
}
