using AutoMapper;
using LBT_Api.Entities;
using LBT_Api.Exceptions;
using LBT_Api.Interfaces.Services;
using LBT_Api.Models.AddressDto;
using LBT_Api.Models.CompanyDto;
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
    /// Unit tests for CompanyService implementation of ICompanyService interface
    /// </summary>
    [TestFixture]
    public class CompanyServiceTests
    {
        private LBT_DbContext _dbContext;
        private ICompanyService _service;

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
            _service = new CompanyService(_dbContext, mapper);
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
            CreateCompanyDto dto = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => _service.Create(dto));
        }

        [Test]
        [Category("Create")]
        public void Create_DtoHasMissingFields_ThrowBadRequestException()
        {
            // Arrange
            CreateCompanyDto dto = new CreateCompanyDto();

            // Assert
            Assert.Throws<BadRequestException>(() => _service.Create(dto));
        }

        [Test]
        [Category("Create")]
        public void Create_DtoIsValid_ReturnGetAddressDto()
        {
            // Arrange
            Company company = Tools.GetExampleCompanyWithDependecies(_dbContext);
            CreateCompanyDto dto = new CreateCompanyDto()
            {
                AddressId = company.AddressId,
                ContactInfoId = company.ContactInfoId,
                Name = company.Name
            };

            // Act
            var result = _service.Create(dto);
            int howManyRecordsAfterOperation = _dbContext.Companys.ToArray().Length;

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
            Company company = Tools.GetExampleCompanyWithDependecies(_dbContext);
            _dbContext.Companys.Add(company);
            _dbContext.SaveChanges();

            // Act
            int result = _service.Delete(company.Id);
            int numberOfRecordsAfterOperation = _dbContext.Companys.ToArray().Length;

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
        public void Read_IdInDb_ReturnGetAddressDto()
        {
            // Arrange
            Company company = Tools.GetExampleCompanyWithDependecies(_dbContext);
            _dbContext.Companys.Add(company);
            _dbContext.SaveChanges();

            // Act
            GetCompanyDto result = _service.Read(company.Id);
            GetCompanyDto companyAsDto = new GetCompanyDto()
            {
                Id = company.Id,
                AddressId = company.AddressId,
                ContactInfoId = company.ContactInfoId,
                Name = company.Name
            };

            // Assert
            Assert.That(result, Is.Not.Null);
            Tools.AssertObjectsAreSameAsJSON(result, companyAsDto);
        }

        // ReadAllTests
        [Test]
        [Category("ReadAll")]
        public void ReadAll_NoRecordsInDb_ReturnEmptyArray()
        {
            // Assert
            int numberOfRecordsInDb = _dbContext.Addresses.ToArray().Length;

            // Act
            GetCompanyDto[] result = _service.ReadAll();

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
                Company company = Tools.GetExampleCompanyWithDependecies(_dbContext);
                _dbContext.Companys.Add(company);
                _dbContext.SaveChanges();
            }
            int howManyRecordsInDb = _dbContext.Addresses.ToArray().Length;

            // Act
            GetCompanyDto[] result = _service.ReadAll();

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
            UpdateCompanyDto dto = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => _service.Update(dto));
        }

        [Test]
        [Category("Update")]
        public void Update_DtoIsMissingId_ThrowBadRequestException()
        {
            // Arrange
            UpdateCompanyDto dto = new UpdateCompanyDto();

            // Assert
            Assert.Throws<BadRequestException>(() => _service.Update(dto));
        }

        [Test]
        [Category("Update")]
        public void Update_IdFromDtoNotInDb_ThrowNotFoundException()
        {
            // Arrange
            UpdateCompanyDto dto = new UpdateCompanyDto()
            {
                Id = -1
            };

            // Assert
            Assert.Throws<NotFoundException>(() => _service.Update(dto));
        }

        [Test]
        [Category("Update")]
        public void Update_DtoIsValid_ReturnGetAddressDto()
        {
            // Arrange
            Company company = Tools.GetExampleCompanyWithDependecies(_dbContext);
            _dbContext.Companys.Add(company);
            _dbContext.SaveChanges();

            UpdateCompanyDto dto = new UpdateCompanyDto()
            {
                Id = company.Id,
                AddressId = company.AddressId,
                ContactInfoId = company.ContactInfoId,
                Name = company.Name + "Updated"
            };

            // Act
            GetCompanyDto result = _service.Update(dto);
            UpdateCompanyDto resultAsDto = new UpdateCompanyDto()
            {
                Id = result.Id,
                AddressId = result.AddressId,
                ContactInfoId = result.ContactInfoId,
                Name = result.Name,
            };

            // Assert
            Tools.AssertObjectsAreSameAsJSON(resultAsDto, dto);
        }
    }
}
