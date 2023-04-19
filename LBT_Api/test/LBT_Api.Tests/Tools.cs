using LBT_Api.Entities;
using LBT_Api.Models.AddressDto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBT_Api.Tests
{
    public static class Tools
    {
        private static string DB_CONNECTION_STRING = "Host=localhost;Port=30001;Database=LBT_DB;Username=AdamDev;Password=AdamDev";
        private const bool IN_MEMORY_DB = true;

        public static LBT_DbContext GetDbContext()
        {
            DbContextOptions<LBT_DbContext> options = null;
            var builder = new DbContextOptionsBuilder<LBT_DbContext>();

            if (IN_MEMORY_DB == true)
                builder.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());
            else
                builder.UseNpgsql(DB_CONNECTION_STRING);

            options = builder.Options;
            LBT_DbContext context = new LBT_DbContext(options);
            context.Database.EnsureCreated();

            return context;
        }

        public static void ClearDbContext(LBT_DbContext context)
        {
            context.Database.EnsureDeleted();
            //context.Dispose();
        }

        public static void IgnoreInMemoryDatabase()
        {
            return;
            if (IN_MEMORY_DB == true)
                Assert.Ignore("Transaction are not supported in in-memory databases");
        }

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

        /// <summary>
        /// Creates dependent rows of ProductSold and returns ProductSold
        /// </summary>
        /// <param name="dbContext">Context for Db</param>
        /// <returns>ProductSold that was not yet added to dbContext</returns>
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

            ProductSold ps = new ProductSold
            {
                AmountSold = 1,
                PriceAtTheTimeOfSale = product.PriceNow,
                ProductId = product.Id,
                SaleId = sale.Id
            };

            return ps;
        }

        /// <summary>
        /// Validates given object
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="obj">Object to be validated</param>
        /// <returns>Validation result</returns>
        /// <exception cref="ArgumentNullException">When obj is null</exception>
        public static bool ModelIsValid<T>(T obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            var context = new ValidationContext(obj, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, context, results, true);
        }
    }
}
