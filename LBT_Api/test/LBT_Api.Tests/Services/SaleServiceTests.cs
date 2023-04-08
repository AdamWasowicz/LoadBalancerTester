using AutoMapper;
using LBT_Api.Entities;
using LBT_Api.Exceptions;
using LBT_Api.Interfaces.Services;
using LBT_Api.Models.ProductSoldDto;
using LBT_Api.Models.SaleDto;
using LBT_Api.Other;
using LBT_Api.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LBT_Api.Tests.Services
{
    /// <summary>
    /// Unit tests for SaleService implementation of ISaleService interface
    /// </summary>
    [TestFixture]
    public class SaleServiceTests
    {
        private LBT_DbContext _dbContext;
        private ISaleService _service;

        [SetUp]
        public void SetUp()
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
            _service = new SaleService(_dbContext, mapper);
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
            CreateSaleDto dto = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => _service.Create(dto));
        }

        [Test]
        [Category("Create")]
        public void Create_DtoHasMissingFields_ThrowBadRequestException()
        {
            // Arrange
            CreateSaleDto dto = new CreateSaleDto();

            // Assert
            Assert.Throws<BadRequestException>(() => _service.Create(dto));
        }

        [Test]
        [Category("Create")]
        [Ignore("Transactions are not supported in-memory databases")]
        public void Create_DtoIsValid_ReturnDto()
        {
            // Arrange
            // Worker
            Worker worker = Tools.GetExampleWorkerWithDependencies(_dbContext);
            _dbContext.Workers.Add(worker);
            _dbContext.SaveChanges();

            // Product
            Product product = Tools.GetExampleProductWithDependencies(_dbContext);
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();

            // CreateProductSoldDto
            CreateProductSold_IntegratedDto[] innerDto = new CreateProductSold_IntegratedDto[]
            {
                new CreateProductSold_IntegratedDto()
                {
                    ProductId = product.Id,
                    AmountSold = 2,
                },

                new CreateProductSold_IntegratedDto()
                {
                    ProductId = product.Id,
                    AmountSold = 3,
                }
            };

            double expectedSumValue = product.PriceNow * innerDto.Sum(x => x.AmountSold);
            
            CreateSaleDto dto = new CreateSaleDto()
            {
                WorkerId = worker.Id,
                ProductsSold = innerDto
            };

            // Act
            int amountOfRowsBeforeAction = _dbContext.Sales.Count();
            GetSaleDto result = _service.Create(dto);
            int amountOfRowsAfterAction = _dbContext.Sales.Count();



            // Assert
            Assert.Greater(amountOfRowsAfterAction, amountOfRowsBeforeAction);
            Assert.That(result.SumValue, Is.EqualTo(expectedSumValue));
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
        [Ignore("Transactions are not supported in-memory databases")]
        public void Delete_IdInDb_ReturnZero()
        {
            // Arrange
            Sale sale = Tools.GetExampleSaleWithDependencies(_dbContext);
            _dbContext.Sales.Add(sale);
            _dbContext.SaveChanges();

            // Act
            int result = _service.Delete(sale.Id);
            int numberOfRecordsAfterOperation = _dbContext.Addresses.ToArray().Length;

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
            Sale sale = Tools.GetExampleSaleWithDependencies(_dbContext);
            _dbContext.Sales.Add(sale);
            _dbContext.SaveChanges();

            // Act
            GetSaleDto result = _service.Read(sale.Id);

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        // ReadAllTests
        [Test]
        [Category("ReadAll")]
        public void ReadAll_NoRecordsInDb_ReturnEmptyArray()
        {
            // Arrange
            int numberOfRecordsInDb = _dbContext.Sales.ToArray().Length;

            // Act
            GetSaleDto[] result = _service.ReadAll();

            // Assert
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
                Sale sale = Tools.GetExampleSaleWithDependencies(_dbContext);
                _dbContext.Sales.Add(sale);
                _dbContext.SaveChanges();
            }
            int howManyRecordsInDb = _dbContext.Sales.ToArray().Length;

            // Act
            GetSaleDto[] result = _service.ReadAll();

            // Assert
            Assert.That(result.Length, Is.EqualTo(howManyToAdd));
            Assert.That(result.Length, Is.EqualTo(howManyRecordsInDb));
        }

        // UpdateTests
        [Test]
        [Category("Update")]
        public void Update_DtoIsNull_ThrowArgumentNullException()
        {
            // Arrange
            UpdateSaleDto dto = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => _service.Update(dto));
        }

        [Test]
        [Category("Update")]
        public void Update_DtoIsMissingId_ThrowBadRequestException()
        {
            // Arrange
            UpdateSaleDto dto = new UpdateSaleDto();

            // Assert
            Assert.Throws<BadRequestException>(() => _service.Update(dto));
        }

        [Test]
        [Category("Update")]
        public void Update_IdFromDtoNotInDb_ThrowNotFoundException()
        {
            // Arrange
            UpdateSaleDto dto = new UpdateSaleDto()
            {
                Id = -1
            };

            // Assert
            Assert.Throws<NotFoundException>(() => _service.Update(dto));
        }

        [Test]
        [Category("Update")]
        public void Update_DtoIsValid_ReturnDto()
        {
            // Arrange
            Sale sale = Tools.GetExampleSaleWithDependencies(_dbContext);
            _dbContext.Sales.Add(sale);
            _dbContext.SaveChanges();

            Worker worker = Tools.GetExampleWorkerWithDependencies(_dbContext);
            _dbContext.Workers.Add(worker);
            _dbContext.SaveChanges();

            UpdateSaleDto dto = new UpdateSaleDto()
            {
                Id = sale.Id,
                WorkerId = worker.Id,
            };

            // Act
            GetSaleDto result = _service.Update(dto);

            // Assert
            Assert.That(result.WorkerId, Is.EqualTo(worker.Id));
        }
    }
}
