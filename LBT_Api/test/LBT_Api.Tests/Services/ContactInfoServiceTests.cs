using AutoMapper;
using LBT_Api.Entities;
using LBT_Api.Exceptions;
using LBT_Api.Interfaces.Services;
using LBT_Api.Models.ContactInfoDto;
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
    /// Unit tests for ContactInfoService implementation of IContactInfoService interface
    /// </summary>
    [TestFixture]
    public class ContactInfoServiceTests
    {
        private LBT_DbContext _dbContext;
        private IContactInfoService _service;

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
            _service = new ContactInfoService(_dbContext, mapper);
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
            CreateContactInfoDto dto = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => _service.Create(dto));
        }

        [Test]
        [Category("Create")]
        public void Create_DtoHasMissingFields_ThrowInvalidModelException()
        {
            // Arrange
            CreateContactInfoDto dto = new CreateContactInfoDto();

            // Assert
            Assert.Throws<InvalidModelException>(() => _service.Create(dto));
        }

        [Test]
        [Category("Create")]
        public void Create_DtoIsValid_ReturnDto()
        {
            // Arrange
            CreateContactInfoDto dto = new CreateContactInfoDto()
            {
                Email = "Email",
                PhoneNumber = "PhoneNumber",
            };
            int howManyRecordsBeforeOperation = _dbContext.ContactInfos.Count();

            // Act
            GetContactInfoDto result = _service.Create(dto);

            // Assert
            int howManyRecordsAfterOperation = _dbContext.ContactInfos.Count();

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
            ContactInfo contactInfo = Tools.GetExampleContactInfo();
            _dbContext.ContactInfos.Add(contactInfo);
            _dbContext.SaveChanges();

            int numberOfRecordsBeforeOperation = _dbContext.ContactInfos.Count();

            // Act
            _service.Delete(contactInfo.Id);

            // Assert
            int numberOfRecordsAfterOperation = _dbContext.Addresses.Count();

            Assert.Multiple(() =>
            {
                Assert.That(numberOfRecordsAfterOperation, Is.EqualTo(0));
                Assert.Greater(numberOfRecordsBeforeOperation, numberOfRecordsAfterOperation);
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
            ContactInfo contactInfo = Tools.GetExampleContactInfo();
            _dbContext.ContactInfos.Add(contactInfo);
            _dbContext.SaveChanges();

            // Act
            GetContactInfoDto result = _service.Read(contactInfo.Id);

            // Assert
            Assert.That(result, Is.Not.Null);
            Tools.AssertObjectsAreSameAsJSON(result, contactInfo);
        }

        // ReadAll
        [Test]
        [Category("ReadAll")]
        public void ReadAll_NoRecordsInDb_ReturnEmptyArray()
        {
            // Assert
            int numberOfRecordsInDb = _dbContext.ContactInfos.ToArray().Length;

            // Act
            GetContactInfoDto[] result = _service.ReadAll();

            // Assert
            Assert.That(result.Length, Is.EqualTo(numberOfRecordsInDb));
        }

        [Test]
        [Category("ReadAll")]
        [TestCase(3)]
        public void ReadAll_RecordsInDb_ReturnDtoArray(int howManyToAdd)
        {
            // Arrange
            for (int i = 0; i < howManyToAdd; i++)
            {
                ContactInfo contactInfo = Tools.GetExampleContactInfo();
                _dbContext.ContactInfos.Add(contactInfo);
                _dbContext.SaveChanges();
            }
            int howManyRecordsInDb = _dbContext.ContactInfos.Count();

            // Act
            GetContactInfoDto[] result = _service.ReadAll();

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
            UpdateContactInfoDto dto = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => _service.Update(dto));
        }

        [Test]
        [Category("Update")]
        public void Update_DtoIsMissingId_ThrowInvalidModelException()
        {
            // Arrange
            UpdateContactInfoDto dto = new UpdateContactInfoDto();

            // Assert
            Assert.Throws<InvalidModelException>(() => _service.Update(dto));
        }

        [Test]
        [Category("Update")]
        public void Update_IdFromDtoNotInDb_ThrowNotFoundException()
        {
            // Arrange
            UpdateContactInfoDto dto = new UpdateContactInfoDto
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
            ContactInfo contactInfo = Tools.GetExampleContactInfo();
            _dbContext.ContactInfos.Add(contactInfo);
            _dbContext.SaveChanges();

            UpdateContactInfoDto dto = new UpdateContactInfoDto()
            {
                Id = contactInfo.Id,
                Email = contactInfo.Email + "Updated",
                PhoneNumber = contactInfo.PhoneNumber + "Updated"
            };

            // Act
            GetContactInfoDto result = _service.Update(dto);
            GetContactInfoDto contactInfoAsDto = new GetContactInfoDto()
            {
                Id = contactInfo.Id,
                Email = contactInfo.Email,
                PhoneNumber = contactInfo.PhoneNumber
            };

            // Assert
            Tools.AssertObjectsAreSameAsJSON(result, contactInfoAsDto);
        }
    }
}
