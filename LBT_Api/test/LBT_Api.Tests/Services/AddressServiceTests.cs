using AutoMapper;
using LBT_Api.Entities;
using LBT_Api.Interfaces.Services;
using LBT_Api.Other;
using LBT_Api.Services;
using LBT_Api.Models.AddressDto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LBT_Api.Exceptions;
using Newtonsoft.Json;
using LBT_Api.Tests;
using Microsoft.AspNetCore.Mvc;

namespace LBT_Api.Tests.Services
{
    /// <summary>
    /// Unit tests for AddressService implementation of IAddressService interface
    /// </summary>
    [TestFixture]
    public class AddressServiceTests
    {
        private LBT_DbContext _dbContext;
        private IAddressService _service;

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
            _service = new AddressService(_dbContext, mapper);
        }

        [TearDown]
        public void TearDown() 
        {
            _dbContext.Dispose();
        }


        // Create
        [Test]
        [Category("Create")]
        public void Create_DtoIsNull_ThrowArgumentNullException()
        {
            // Arrange
            CreateAddressDto dto = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => _service.Create(dto));
        }

        [Test]
        [Category("Create")]
        public void Create_DtoHasMissingFields_ThrowInvalidModelException() 
        {
            // Arrange
            CreateAddressDto dto = new CreateAddressDto();

            // Assert
            Assert.Throws<InvalidModelException>(() => _service.Create(dto));
        }

        [Test]
        [Category("Create")]
        public void Create_DtoIsValid_ReturnDto()
        {
            // Arrange
            CreateAddressDto dto = new CreateAddressDto()
            {
                Country = "Country",
                City = "City",
                Street = "Street",
                BuildingNumber = "BuildingNumber",
            };

            int howManyRecordsBeforeOperation = _dbContext.Addresses.Count();

            // Act
            GetAddressDto result = _service.Create(dto);

            // Assert
            int howManyRecordsAfterOperation = _dbContext.Addresses.Count();

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(howManyRecordsAfterOperation, Is.EqualTo(1));
                Assert.Greater(howManyRecordsAfterOperation, howManyRecordsBeforeOperation);
            });
        }

        // Delete
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
        public void Delete_IdInDb_Return()
        {
            // Arrange
            Address address = Tools.GetExampleAddress();
            _dbContext.Addresses.Add(address);
            _dbContext.SaveChanges();

            int numberOfRcordsBeforeOperation = _dbContext.Addresses.Count();

            // Act
            _service.Delete(address.Id);

            // Assert
            int numberOfRecordsAfterOperation = _dbContext.Addresses.Count();

            Assert.Multiple(() =>
            {
                Assert.That(numberOfRecordsAfterOperation, Is.EqualTo(0));
                Assert.Greater(numberOfRcordsBeforeOperation, numberOfRecordsAfterOperation);
            });
        }

        // Read
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
            Address address = Tools.GetExampleAddress();
            _dbContext.Addresses.Add(address);
            _dbContext.SaveChanges();

            // Act
            GetAddressDto result = _service.Read(address.Id);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Tools.AssertObjectsAreSameAsJSON(result, address);
            });
        }

        // ReadAll
        [Test]
        [Category("ReadAll")]
        public void ReadAll_NoRecordsInDb_ReturnEmptyArray()
        {
            // Assert
            int numberOfRecordsInDb = _dbContext.Addresses.ToArray().Length;

            // Act
            GetAddressDto[] result = _service.ReadAll();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Length, Is.EqualTo(numberOfRecordsInDb));
                Assert.That(result.Length, Is.EqualTo(0));
            });
        }

        [Test]
        [Category("ReadAll")]
        [TestCase(3)]
        public void ReadAll_RecordsInDb_ReturnDtoArray(int howManyToAdd)
        {
            // Arrange
            for (int i = 0; i < howManyToAdd; i++)
            {
                Address address = Tools.GetExampleAddress();
                _dbContext.Addresses.Add(address);
                _dbContext.SaveChanges();
            }
            int howManyRecordsInDb = _dbContext.Addresses.ToArray().Length;

            // Act
            GetAddressDto[] result = _service.ReadAll();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Length, Is.EqualTo(howManyToAdd));
                Assert.That(result.Length, Is.EqualTo(howManyRecordsInDb));
            });
        }

        // Update
        [Test]
        [Category("Update")]
        public void Update_DtoIsNull_ThrowArgumentNullException()
        {
            // Arrange
            UpdateAddressDto dto = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => _service.Update(dto));
        }

        [Test]
        [Category("Update")]
        public void Update_DtoIsMissingId_ThrowInvalidModelException()
        {
            // Arrange
            UpdateAddressDto dto = new UpdateAddressDto();

            // Assert
            Assert.Throws<InvalidModelException>(() =>  _service.Update(dto));
        }

        [Test]
        [Category("Update")]
        public void Update_IdFromDtoNotInDb_ThrowNotFoundException()
        {
            // Arrange
            UpdateAddressDto dto = new UpdateAddressDto
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
            Address address = Tools.GetExampleAddress();
            _dbContext.Addresses.Add(address);
            _dbContext.SaveChanges();

            UpdateAddressDto dto = new UpdateAddressDto()
            {
                Id = address.Id,
                Country = address.Country + "Updated",
                City = address.City + "Updated",
                Street = address.Street + "Updated",
                BuildingNumber = address.BuildingNumber + "Updated",
            };

            // Act
            GetAddressDto result = _service.Update(dto);
            GetAddressDto addressAsDto = new GetAddressDto()
            {
                Id = address.Id,
                Country = address.Country,
                City = address.City,
                Street = address.Street,
                BuildingNumber = address.BuildingNumber,
            };

            // Assert
            Tools.AssertObjectsAreSameAsJSON(result, addressAsDto);
        }
    }
}
