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
    [TestFixture]
    public class AddressServiceTests
    {
        private LBT_DbContext _dbContext;
        private IAddressService _addressService;

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
            _addressService = new AddressService(_dbContext, mapper);
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
            CreateAddressDto dto = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => _addressService.Create(dto));
        }

        [Test]
        [Category("Create")]
        public void Create_DtoHasMissingFields_ThrowBadRequestException() 
        {
            // Arrange
            CreateAddressDto dto = new CreateAddressDto();

            // Assert
            Assert.Throws<BadRequestException>(() => _addressService.Create(dto));
        }

        [Test]
        [Category("Create")]
        public void Create_DtoIsValid_ReturnGetAddressDto()
        {
            // Arrange
            CreateAddressDto dto = Tools.GetExampleCreateAddressDto();

            // Act
            GetAddressDto result = _addressService.Create(dto);
            int howManyRecordsAfterOperation = _dbContext.Addresses.ToArray().Length;

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
            Assert.Throws<NotFoundException>(() => _addressService.Delete(searchedId));
        }

        [Test]
        [Category("Delete")]
        public void Delete_IdInDb_ReturnZero()
        {
            // Arrange
            Address address = Tools.GetExampleAddress();
            _dbContext.Addresses.Add(address);
            _dbContext.SaveChanges();
            int idOfCreatedRecord = address.Id;

            // Act
            int result = _addressService.Delete(idOfCreatedRecord);
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
            Assert.Throws<NotFoundException>(() => _addressService.Read(searchedId));
        }

        [Test]
        [Category("Read")]
        public void Read_IdInDb_ReturnGetAddressDto()
        {
            // Arrange
            Address address = Tools.GetExampleAddress();
            _dbContext.Addresses.Add(address);
            _dbContext.SaveChanges();
            int idOfCreatedRecord = address.Id;

            // Act
            GetAddressDto result = _addressService.Read(idOfCreatedRecord);

            // Assert
            Assert.That(result, Is.Not.Null);
            Tools.AssertObjectsAreSameAsJSON(result, address);
        }

        // ReadAllTests
        [Test]
        [Category("ReadAll")]
        public void ReadAll_NoRecordsInDb_ReturnEmptyArray()
        {
            // Assert
            int numberOfRecordsInDb = _dbContext.Addresses.ToArray().Length;

            // Act
            GetAddressDto[] result = _addressService.ReadAll();

            // Assert
            Assert.That(result.Length, Is.EqualTo(numberOfRecordsInDb));
        }

        [Test]
        [Category("ReadAll")]
        [TestCase(3)]
        public void ReadAll_RecordsInDb_ReturnArrayOfGetAddressDto(int howManyToAdd)
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
            GetAddressDto[] result = _addressService.ReadAll();

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
            UpdateAddressDto dto = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => _addressService.Update(dto));
        }

        [Test]
        [Category("Update")]
        public void Update_DtoIsMissingId_ThrowBadRequestException()
        {
            // Arrange
            UpdateAddressDto dto = new UpdateAddressDto();

            // Assert
            Assert.Throws<BadRequestException>(() =>  _addressService.Update(dto));
        }

        [Test]
        [Category("Update")]
        public void Update_IdFromDtoNotInDb_ThrowNotFoundException()
        {
            // Arrange
            UpdateAddressDto dto = new UpdateAddressDto()
            {
                Id = -1,
            };

            // Assert
            Assert.Throws<NotFoundException>(() => _addressService.Update(dto));
        }

        [Test]
        [Category("Update")]
        public void Update_DtoIsValid_ReturnGetAddressDto()
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
            GetAddressDto result = _addressService.Update(dto);

            // Assert
            Tools.AssertObjectsAreSameAsJSON(result, address);
        }
    }
}
