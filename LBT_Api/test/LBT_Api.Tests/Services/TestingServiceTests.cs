using AutoMapper;
using LBT_Api.Entities;
using LBT_Api.Interfaces.Services;
using LBT_Api.Other;
using LBT_Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBT_Api.Tests.Services
{
    [TestFixture]
    public class TestingServiceTests
    {
        private LBT_DbContext _dbContext;
        private ITestingService _service;
        private ICompanyService _companyService;


        [SetUp]
        public void SetUp()
        {
            // Set up db
            _dbContext = Tools.GetDbContext();

            // AutoMapper
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<LBT_Entity_MappingProfile>());
            IMapper mapper = mapperConfig.CreateMapper();

            // Service
            _service = new TestingService(_dbContext);
            _companyService = new CompanyService(_dbContext, mapper);
        }

        [TearDown]
        public void TearDown()
        {
            Tools.ClearDbContext(_dbContext);
        }

        [Test]
        public void ClearAll_Clears_Return()
        {
            // Arrange
            _companyService.CreateExampleData(50);

            // Act
            _service.ClearAll();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(0, Is.EqualTo(_dbContext.Companys.Count()), message: "Companys");
                Assert.That(0, Is.EqualTo(_dbContext.Workers.Count()), message: "Workers");
                Assert.That(0, Is.EqualTo(_dbContext.ContactInfos.Count()), message: "ContactInfos");
                Assert.That(0, Is.EqualTo(_dbContext.Addresses.Count()), message: "Addresses");
                Assert.That(0, Is.EqualTo(_dbContext.Sales.Count()), message: "Sales");
                Assert.That(0, Is.EqualTo(_dbContext.ProductsSold.Count()), message: "ProductsSold");
                Assert.That(0, Is.EqualTo(_dbContext.Products.Count()), message: "Products");
                Assert.That(0, Is.EqualTo(_dbContext.Suppliers.Count()), message: "Suppliers");
            });
        }
    }
}
