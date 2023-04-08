using AutoMapper;
using LBT_Api.Entities;
using LBT_Api.Exceptions;
using LBT_Api.Interfaces.Services;
using LBT_Api.Models.AddressDto;
using LBT_Api.Models.SupplierDto;
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
    /// Unit tests for SupplierService implementation of ISupplierService interface
    /// </summary>
    [TestFixture]
    public class SupplierServiceTests
    {
        private LBT_DbContext _dbContext;
        private ISupplierService _service;

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
            _service = new SupplierService(_dbContext, mapper);
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
            CreateSupplierDto dto = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => _service.Create(dto));
        }

        [Test]
        [Category("Create")]
        public void Create_DtoHasMissingFields_ThrowBadRequestException()
        {
            // Arrange
            CreateSupplierDto dto = new CreateSupplierDto();

            // Assert
            Assert.Throws<BadRequestException>(() => _service.Create(dto));
        }

        [Test]
        [Category("Create")]
        public void Create_DtoIsValid_ReturnDto()
        {
            // Arrange
            Address address = Tools.GetExampleAddress();
            _dbContext.Addresses.Add(address);
            _dbContext.SaveChanges();

            CreateSupplierDto dto = new CreateSupplierDto()
            {
                Name = "Name",
                AddressId = address.Id
            };

            // Act
            GetSupplierDto result = _service.Create(dto);
            int howManyRecordsAfterOperation = _dbContext.Suppliers.ToArray().Length;

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
            Supplier supplier = Tools.GetExampleSupplierWithDependencies(_dbContext);
            _dbContext.Suppliers.Add(supplier);
            _dbContext.SaveChanges();

            // Act
            int result = _service.Delete(supplier.Id);
            int numberOfRecordsAfterOperation = _dbContext.Suppliers.ToArray().Length;

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
            Supplier supplier = Tools.GetExampleSupplierWithDependencies(_dbContext);
            _dbContext.Suppliers.Add(supplier);
            _dbContext.SaveChanges();

            // Act
            GetSupplierDto result  = _service.Read(supplier.Id);


            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(supplier.Id, Is.EqualTo(result.Id));
        }

        // ReadAllTests
        [Test]
        [Category("ReadAll")]
        public void ReadAll_NoRecordsInDb_ReturnEmptyArray()
        {
            // Assert
            int numberOfRecordsInDb = _dbContext.Suppliers.ToArray().Length;

            // Act
            GetSupplierDto[] result = _service.ReadAll();

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
                Supplier supplier = Tools.GetExampleSupplierWithDependencies(_dbContext);
                _dbContext.Suppliers.Add(supplier);
                _dbContext.SaveChanges();
            }
            int howManyRecordsInDb = _dbContext.Suppliers.Count();

            // Act
            GetSupplierDto[] result = _service.ReadAll();

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
            UpdateSupplierDto dto = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => _service.Update(dto));
        }

        [Test]
        [Category("Update")]
        public void Update_DtoIsMissingId_ThrowBadRequestException()
        {
            // Arrange
            UpdateSupplierDto dto = new UpdateSupplierDto();

            // Assert
            Assert.Throws<BadRequestException>(() => _service.Update(dto));
        }

        [Test]
        [Category("Update")]
        public void Update_IdFromDtoNotInDb_ThrowNotFoundException()
        {
            // Arrange
            UpdateSupplierDto dto = new UpdateSupplierDto()
            {
                Id = -1,
            };

            // Assert
            Assert.Throws<NotFoundException>(() => _service.Update(dto));
        }

        [Test]
        [Category("Update")]
        public void Update_DtoIsValid_ReturnDto()
        {
            // Arrange
            Supplier supplier = Tools.GetExampleSupplierWithDependencies(_dbContext);
            _dbContext.Suppliers.Add(supplier);
            _dbContext.SaveChanges();

            UpdateSupplierDto dto = new UpdateSupplierDto()
            {
                Id = supplier.Id,
                AddressId = supplier.AddressId,
                Name = supplier.Name + "Updated"
            };

            // Act
            GetSupplierDto result = _service.Update(dto);
            GetSupplierDto supplierAsDto = new GetSupplierDto()
            {
                Id = supplier.Id,
                AddressId = supplier.AddressId,
                Name = supplier.Name
            };

            // Assert
            Tools.AssertObjectsAreSameAsJSON(result, supplierAsDto);
        }
    }
}
