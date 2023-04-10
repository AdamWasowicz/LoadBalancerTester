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
        public static string AsJSON(object obj)
        {
            string output = JsonConvert.SerializeObject(obj);
            return output;
        }

        public static void AssertObjectsAreSameAsJSON(object obj1, object obj2)
        {
            Assert.That(AsJSON(obj1), Is.EqualTo(AsJSON(obj2)));
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
            Company company = new Company()
            {
                AddressId = address.Id,
                ContactInfoId = contactInfo.Id,
                Name = "Name",
            };

            return company; ;

            return company;
        }

        public static ContactInfo GetExampleContactInfo()
        {
            ContactInfo contactInfo = new ContactInfo()
            {
                Email = "Email",
                PhoneNumber = "PhoneNumber",
            };

            return contactInfo;
        }

        /// <summary>
        /// Creates dependent rows of Product and returns Product
        /// </summary>
        /// <param name="dbContext">Context for Db</param>
        /// <returns>Product that was not yet added to dbContext</returns>
        public static Product GetExampleProductWithDependencies(LBT_DbContext dbContext)
        {
            // Supplier
            Supplier supplier = GetExampleSupplierWithDependencies(dbContext);
            dbContext.Suppliers.Add(supplier);
            dbContext.SaveChanges();

            // Product
            Product product = new Product()
            {
                Name = "Name",
                PriceNow = 10,
                SupplierId = supplier.Id,
            };

            return product;
        }

        /// <summary>
        /// Creates dependent rows of Supplier and returns Supplier
        /// </summary>
        /// <param name="dbContext">Context for Db</param>
        /// <returns>Supplier that was not yet added to dbContext</returns>
        public static Supplier GetExampleSupplierWithDependencies(LBT_DbContext dbContext)
        {
            // Address
            Address address = GetExampleAddress();
            dbContext.Addresses.Add(address);
            dbContext.SaveChanges();

            // Supplier
            Supplier supplier = new Supplier()
            {
                Name = "Name",
                AddressId = address.Id,
            };

            return supplier;
        }

        /// <summary>
        /// Creates dependent rows of Sale and returns Sale
        /// </summary>
        /// <param name="dbContext">Context for Db</param>
        /// <returns>Sale that was not yet added to dbContext</returns>
        public static Sale GetExampleSaleWithDependencies(LBT_DbContext dbContext)
        {
            // Worker
            Worker worker = GetExampleWorkerWithDependencies(dbContext);
            dbContext.Workers.Add(worker);
            dbContext.SaveChanges();

            Sale sale = new Sale()
            {
                SaleDate = DateTime.Now,
                WorkerId = worker.Id,
                SumValue = 0
            };

            return sale;
        }

        /// <summary>
        /// Creates dependent rows of Worker and returns Worker
        /// </summary>
        /// <param name="dbContext">Context for Db</param>
        /// <returns>Worker that was not yet added to dbContext</returns>
        public static Worker GetExampleWorkerWithDependencies(LBT_DbContext dbContext)
        {
            // Company
            Company company = GetExampleCompanyWithDependecies(dbContext);
            dbContext.Companys.Add(company);
            dbContext.SaveChanges();

            // Address
            Address address = GetExampleAddress();
            dbContext.Addresses.Add(address);
            dbContext.SaveChanges();

            // ContactInfo
            ContactInfo contactInfo = GetExampleContactInfo();
            dbContext.ContactInfos.Add(contactInfo);
            dbContext.SaveChanges();

            Worker worker = new Worker()
            {
                CompanyId = company.Id,
                AddressId = address.Id,
                ContactInfoId = contactInfo.Id,
                Name = "Name",
                Surname = "Surname",
            };

            return worker;
        }

        public static ProductSold GetExampleProductSoldWithDependencies(LBT_DbContext dbContext)
        {
            // Product
            Product product = GetExampleProductWithDependencies(dbContext);
            dbContext.Products.Add(product);
            dbContext.SaveChanges();

            // Sale
            Sale sale = GetExampleSaleWithDependencies(dbContext);
            dbContext.Sales.Add(sale);
            dbContext.SaveChanges();

            ProductSold ps = new ProductSold()
            {
                AmountSold = 1,
                PriceAtTheTimeOfSale = product.PriceNow,
                ProductId = product.Id,
                SaleId = sale.Id
            };

            return ps;
        }
    }
}
