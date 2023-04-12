using AutoMapper;
using LBT_Api.Entities;
using LBT_Api.Exceptions;
using LBT_Api.Interfaces.Services;
using LBT_Api.Models.ProductDto;
using LBT_Api.Models.ProductSoldDto;
using LBT_Api.Other;
using LBT_Api.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LBT_Api.Tests.Services
{
    /// <summary>
    /// Unit tests for ProductSold implementation of IProductSold interface
    /// </summary>
    [TestFixture]
    public class ProductSoldServiceTests
    {
        private LBT_DbContext _dbContext;
        private IProductSoldService _service;

        [SetUp]
        public void Setup()
        {
            // Set up in-memory database
            var builder = new DbContextOptionsBuilder<LBT_DbContext>();
            builder.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());
            var dbContextOptions = builder.Options;
            _dbContext = new LBT_DbContext(dbContextOptions);

            // AutoMapper
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<LBT_Entity_MappingProfile>());
            IMapper mapper = mapperConfig.CreateMapper();

            // Service
            _service = new ProductSoldService(_dbContext, mapper);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Dispose();
        }

        // CreateTests
        [Test]
        [Category("Create")]
        public void Create_DtoIsNull_ThrowArgumentNullException()
        {
            // Arrange
            CreateProductSoldDto dto = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => _service.Create(dto));
        }

        [Test]
        [Category("Create")]
        public void Create_DtoHasMissingFields_ThrowBadRequestException()
        {
            // Arrange
            CreateProductSoldDto dto = new CreateProductSoldDto();

            // Assert
            Assert.Throws<BadRequestException>(() => _service.Create(dto));
        }

        [Test]
        [Category("Create")]
        public void Create_DtoIsValid_ReturnDto()
        {
            // Arrange
            Product product = Tools.GetExampleProductWithDependencies(_dbContext);
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();

            Sale sale = Tools.GetExampleSaleWithDependencies(_dbContext);
            _dbContext.Sales.Add(sale);
            _dbContext.SaveChanges();

            CreateProductSoldDto dto = new CreateProductSoldDto()
            {
                AmountSold = 1,
                ProductId = product.Id,
                SaleId = sale.Id
            };

            // Act
            GetProductSoldDto result = _service.Create(dto);

            // Assert
            double saleSumValue = _dbContext.Sales.FirstOrDefault(s => s.Id == sale.Id).SumValue;
            double expectedSumValue = _dbContext.ProductsSold.Where(ps => ps.SaleId == sale.Id).Sum(ps => ps.AmountSold * ps.PriceAtTheTimeOfSale);
            int amountOfRowsInDb = _dbContext.ProductsSold.Count();

            Assert.That(saleSumValue, Is.EqualTo(expectedSumValue));
            Assert.That(amountOfRowsInDb, Is.EqualTo(1));
        }

        // DeleteTests
        [Test]
        [Category("Delete")]
        public void Delete_IdNotInDb_ThrowNotFoundException()
        {
            // Arrange
            int searchedId = -1;

            // Assert
            Assert.Throws<NotFoundException>(() => _service.Delete(searchedId));
        }

        [Test]
        [Category("Delete")]
        public void Delete_IdInDb_ReturnZero()
        {
            // Arrange
            ProductSold ps = Tools.GetExampleProductSoldWithDependencies(_dbContext);
            _dbContext.ProductsSold.Add(ps);
            _dbContext.SaveChanges();

            // Act
            int result = _service.Delete(ps.Id);
            int numberOfRecordsAfterOperation = _dbContext.ProductsSold.ToArray().Length;

            // Assert
            Assert.That(result, Is.EqualTo(0));
            Assert.That(numberOfRecordsAfterOperation, Is.EqualTo(0));
        }

        // ReadTests
        [Test]
        [Category("Read")]
        public void Read_IdNotInDb_ThrowNotFoundException()
        {
            // Arrange
            int searchedId = -1;

            // Assert
            Assert.Throws<NotFoundException>(() => _service.Read(searchedId));
        }

        [Test]
        [Category("Read")]
        public void Read_IdInDb_ReturnDto()
        {
            // Arrange
            ProductSold ps = Tools.GetExampleProductSoldWithDependencies(_dbContext);
            _dbContext.ProductsSold.Add(ps);
            _dbContext.SaveChanges();

            // Act
            GetProductSoldDto result = _service.Read(ps.Id);

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        // ReadAllTests
        [Test]
        [Category("ReadAll")]
        public void ReadAll_NoRecordsInDb_ReturnEmptyArray()
        {
            // Assert
            int numberOfRecordsInDb = _dbContext.Addresses.ToArray().Length;

            // Act
            GetProductSoldDto[] result = _service.ReadAll();

            Assert.That(result.Length, Is.EqualTo(numberOfRecordsInDb));
            Assert.That(result.Length, Is.EqualTo(0));
        }

        [Test]
        [Category("ReadAll")]
        [TestCase(3)]
        public void ReadAll_RecordsInDb_ReturnDtoArray(int howManyToAdd)
        {
            // Arrange
            for (int i = 0; i < howManyToAdd; i++)
            {
                ProductSold ps = Tools.GetExampleProductSoldWithDependencies(_dbContext);
                _dbContext.ProductsSold.Add(ps);
                _dbContext.SaveChanges();
            }
            int howManyRecordsInDb = _dbContext.ProductsSold.ToArray().Length;

            // Act
            GetProductSoldDto[] result = _service.ReadAll();

            // Assert
            Assert.That(result.Length, Is.EqualTo(howManyToAdd));
            Assert.That(result.Length, Is.EqualTo(howManyRecordsInDb));
        }

        // UpdateTests
        [Test]
        [Category("UpdateName")]
        public void Update_DtoIsNull_ThrowArgumentNullException()
        {
            // Arrange
            UpdateProductSoldDto dto = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => _service.Update(dto));
        }

        [Test]
        [Category("UpdateName")]
        public void Update_DtoIsMissingId_ThrowBadRequestException()
        {
            // Arrange
            UpdateProductSoldDto dto = new UpdateProductSoldDto();

            // Assert
            Assert.Throws<BadRequestException>(() => _service.Update(dto));
        }

        [Test]
        [Category("UpdateName")]
        public void Update_IdFromDtoNotInDb_ThrowNotFoundException()
        {
            // Arrange
            UpdateProductSoldDto dto = new UpdateProductSoldDto()
            {
                Id = -1
            };

            // Assert
            Assert.Throws<NotFoundException>(() => _service.Update(dto));
        }

        [Test]
        [Category("UpdateName")]
        public void Update_DtoIsValid_ReturnDto()
        {
            // Arrange
            ProductSold ps = Tools.GetExampleProductSoldWithDependencies(_dbContext);
            _dbContext.ProductsSold.Add(ps);
            _dbContext.SaveChanges();

            UpdateProductSoldDto dto = new UpdateProductSoldDto()
            {
                Id = ps.Id,
                AmountSold = ps.AmountSold + 1,
                PriceAtTheTimeOfSale = ps.PriceAtTheTimeOfSale + 1,
                ProductId = ps.ProductId,
                SaleId = ps.SaleId
            };

            // Act
            GetProductSoldDto result = _service.Update(dto);

            // Assert
            double saleSumValue = _dbContext.Sales.FirstOrDefault(s => s.Id == ps.SaleId).SumValue;
            double expectedSumValue = _dbContext.ProductsSold.Where(ps => ps.SaleId == ps.SaleId).Sum(ps => ps.AmountSold * ps.PriceAtTheTimeOfSale);
            int amountOfRowsInDb = _dbContext.ProductsSold.Count();

            Assert.That(saleSumValue, Is.EqualTo(expectedSumValue));
            Assert.That(amountOfRowsInDb, Is.EqualTo(1));
        }
    }
}
