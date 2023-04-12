using AutoMapper;
using LBT_Api.Entities;
using LBT_Api.Exceptions;
using LBT_Api.Interfaces.Services;
using LBT_Api.Models.ProductDto;
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
    /// Unit tests for ProductService implementation of IProductService interface
    /// </summary>
    [TestFixture]
    public class ProductServiceTests
    {
        private LBT_DbContext _dbContext;
        private IProductService _service;

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
            _service = new ProductService(_dbContext, mapper);
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
            CreateProductDto dto = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => _service.Create(dto));
        }

        [Test]
        [Category("Create")]
        public void Create_DtoHasMissingFields_ThrowBadRequestException()
        {
            // Arrange
            CreateProductDto dto = new CreateProductDto();

            // Assert
            Assert.Throws<BadRequestException>(() => _service.Create(dto));
        }

        [Test]
        [Category("Create")]
        public void Create_DtoIsValid_ReturnDto()
        {
            // Arrange
            Supplier supplier = Tools.GetExampleSupplierWithDependencies(_dbContext);
            _dbContext.Suppliers.Add(supplier);
            _dbContext.SaveChanges();

            CreateProductDto dto = new CreateProductDto()
            {
                Name = "Name",
                PriceNow = 10,
                SupplierId = supplier.Id,
            };

            // Act
            GetProductDto result = _service.Create(dto);
            int howManyRecordsAfterOperation = _dbContext.Products.ToArray().Length;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(howManyRecordsAfterOperation, Is.EqualTo(1));
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
            Product product = Tools.GetExampleProductWithDependencies(_dbContext);
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();

            // Act
            int result = _service.Delete(product.Id);
            int numberOfRecordsAfterOperation = _dbContext.Products.ToArray().Length;

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
            Product product = Tools.GetExampleProductWithDependencies(_dbContext);
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();

            // Act
            GetProductDto result = _service.Read(product.Id);
            GetProductDto productAsDto = new GetProductDto()
            {
                Id = product.Id,
                Name = product.Name,
                PriceNow = product.PriceNow,
                SupplierId = product.SupplierId
            };

            // Assert
            Assert.That(result, Is.Not.Null);
            Tools.AssertObjectsAreSameAsJSON(result, productAsDto);
        }

        // ReadAllTests
        [Test]
        [Category("ReadAll")]
        public void ReadAll_NoRecordsInDb_ReturnEmptyArray()
        {
            // Assert
            int numberOfRecordsInDb = _dbContext.Products.ToArray().Length;

            // Act
            GetProductDto[] result = _service.ReadAll();

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
                Product product = Tools.GetExampleProductWithDependencies(_dbContext);
                _dbContext.Products.Add(product);
                _dbContext.SaveChanges();
            }
            int howManyRecordsInDb = _dbContext.Products.ToArray().Length;

            // Act
            GetProductDto[] result = _service.ReadAll();

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
            UpdateProductDto dto = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => _service.Update(dto));
        }

        [Test]
        [Category("UpdateName")]
        public void Update_DtoIsMissingId_ThrowBadRequestException()
        {
            // Arrange
            UpdateProductDto dto = new UpdateProductDto();

            // Assert
            Assert.Throws<BadRequestException>(() => _service.Update(dto));
        }

        [Test]
        [Category("UpdateName")]
        public void Update_IdFromDtoNotInDb_ThrowNotFoundException()
        {
            // Arrange
            UpdateProductDto dto = new UpdateProductDto()
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
            Product product = Tools.GetExampleProductWithDependencies(_dbContext);
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();

            UpdateProductDto dto = new UpdateProductDto()
            {
                Id = product.Id,
                Name = product.Name + "Updated",
                PriceNow = product.PriceNow + 1,
                SupplierId = product.SupplierId
            };

            // Act
            GetProductDto result = _service.Update(dto);
            GetProductDto productAsDto = new GetProductDto()
            {
                Id = product.Id,
                Name = product.Name,
                PriceNow = product.PriceNow,
                SupplierId = product.SupplierId
            };

            // Assert
            Tools.AssertObjectsAreSameAsJSON(result, productAsDto);
        }
    }
}
