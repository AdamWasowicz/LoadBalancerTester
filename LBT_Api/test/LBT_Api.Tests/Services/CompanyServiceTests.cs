using AutoMapper;
using LBT_Api.Entities;
using LBT_Api.Exceptions;
using LBT_Api.Interfaces.Services;
using LBT_Api.Models.AddressDto;
using LBT_Api.Models.CompanyDto;
using LBT_Api.Models.ContactInfoDto;
using LBT_Api.Other;
using LBT_Api.Services;
using Microsoft.EntityFrameworkCore;


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
            // Set up db
            _dbContext = Tools.GetDbContext();

            // AutoMapper
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<LBT_Entity_MappingProfile>());
            IMapper mapper = mapperConfig.CreateMapper();

            // Service
            _service = new CompanyService(_dbContext, mapper);
        }

        [TearDown]
        public void TearDown()
        {
            Tools.ClearDbContext(_dbContext);
        }

        // Create
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
        public void Create_DtoHasMissingFields_ThrowInvalidModelException()
        {
            // Arrange
            CreateCompanyDto dto = new CreateCompanyDto();

            // Assert
            Assert.Throws<InvalidModelException>(() => _service.Create(dto));
        }

        [Test]
        [Category("Create")]
        public void Create_DtoIsValid_ReturnDto()
        {
            // Arrange
            Company company = Tools.GetExampleCompanyWithDependecies(_dbContext);
            CreateCompanyDto dto = new CreateCompanyDto()
            {
                AddressId = company.AddressId,
                ContactInfoId = company.ContactInfoId,
                Name = company.Name
            };

            int howManyRecordsBeforeOperation = _dbContext.Companys.Count();

            // Act
            var result = _service.Create(dto);

            // Assert
            int howManyRecordsAfterOperation = _dbContext.Companys.Count();

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
            Company company = Tools.GetExampleCompanyWithDependecies(_dbContext);
            _dbContext.Companys.Add(company);
            _dbContext.SaveChanges();

            int numberOfRecordsBeforeOperation = _dbContext.Companys.Count();

            // Act
            _service.Delete(company.Id);

            // Assert
            int numberOfRecordsAfterOperation = _dbContext.Companys.Count();

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
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Tools.AssertObjectsAreSameAsJSON(result, companyAsDto);
            });
        }

        // ReadAll
        [Test]
        [Category("ReadAll")]
        public void ReadAll_NoRecordsInDb_ReturnEmptyArray()
        {
            // Assert
            int numberOfRecordsInDb = _dbContext.Addresses.Count();

            // Act
            GetCompanyDto[] result = _service.ReadAll();

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
                Company company = Tools.GetExampleCompanyWithDependecies(_dbContext);
                _dbContext.Companys.Add(company);
                _dbContext.SaveChanges();
            }
            int howManyRecordsInDb = _dbContext.Addresses.Count();

            // Act
            GetCompanyDto[] result = _service.ReadAll();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Length, Is.EqualTo(howManyToAdd));
                Assert.That(result.Length, Is.EqualTo(howManyRecordsInDb));
            });
        }

        // UpdateName
        [Test]
        [Category("UpdateName")]
        public void UpdateName_DtoIsNull_ThrowArgumentNullException()
        {
            // Arrange
            UpdateCompanyNameDto dto = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => _service.UpdateName(dto));
        }

        [Test]
        [Category("UpdateName")]
        public void UpdateName_DtoIsMissingId_ThrowInvalidModelException()
        {
            // Arrange
            UpdateCompanyNameDto dto = new UpdateCompanyNameDto();

            // Assert
            Assert.Throws<InvalidModelException>(() => _service.UpdateName(dto));
        }

        [Test]
        [Category("UpdateName")]
        public void UpdateName_IdFromDtoNotInDb_ThrowNotFoundException()
        {
            // Arrange
            UpdateCompanyNameDto dto = new UpdateCompanyNameDto()
            {
                Id = -1,
                Name = "Name"
            };

            // Assert
            Assert.Throws<NotFoundException>(() => _service.UpdateName(dto));
        }

        [Test]
        [Category("UpdateName")]
        public void UpdateName_IdInDbAndNameNotInDto_ThrowInvalidModelException()
        {
            // Arrange
            Company company = Tools.GetExampleCompanyWithDependecies(_dbContext);
            _dbContext.Companys.Add(company);
            _dbContext.SaveChanges();

            UpdateCompanyNameDto dto = new UpdateCompanyNameDto()
            {
                Id = company.Id,
            };

            // Assert
            Assert.Throws<InvalidModelException>(() => _service.UpdateName(dto));
        }

        [Test]
        [Category("UpdateName")]
        public void UpdateName_IdNotInDbAndNameNotInDto_ThrowInvalidModelException()
        {
            UpdateCompanyNameDto dto = new UpdateCompanyNameDto()
            {
                Id = -1,
            };

            // Assert
            Assert.Throws<InvalidModelException>(() => _service.UpdateName(dto));
        }

        [Test]
        [Category("UpdateName")]
        public void UpdateName_IdNotInDtoAndNameNotInDto_ThrowInvalidModelException()
        {
            UpdateCompanyNameDto dto = new UpdateCompanyNameDto();

            // Assert
            Assert.Throws<InvalidModelException>(() => _service.UpdateName(dto));
        }

        [Test]
        [Category("UpdateName")]
        public void UpdateName_DtoIsValid_ReturnDto()
        {
            // Arrange
            Company company = Tools.GetExampleCompanyWithDependecies(_dbContext);
            _dbContext.Companys.Add(company);
            _dbContext.SaveChanges();

            UpdateCompanyNameDto dto = new UpdateCompanyNameDto()
            {
                Id = company.Id,
                Name = company.Name + "Updated"
            };

            // Act
            GetCompanyDto result = _service.UpdateName(dto);

            // Assert
            Tools.AssertObjectsAreSameAsJSON(result.Name, dto.Name);
        }

        // CreateWithDependencies
        [Test]
        [Category("CreateWithDependencies")]
        //[Ignore("Transaction are not supported in-memory databases")]
        public void CreateWithDependencies_DtoIsNull_ThrowArgumenNullException()
        {
            Tools.IgnoreInMemoryDatabase();

            // Arrange
            CreateCompanyWithDependenciesDto dto = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => _service.CreateWithDependencies(dto));
        }

        [Test]
        [Category("CreateWithDependencies")]
        //[Ignore("Transaction are not supported in-memory databases")]
        public void CreateWithDependencies_AddressInDtoIsNull_ThrowInvalidModelException()
        {
            Tools.IgnoreInMemoryDatabase();

            // Arrange
            CreateCompanyWithDependenciesDto dto = new CreateCompanyWithDependenciesDto
            {
                Name = "CompanyName",

                Address = null,

                ContactInfo = new CreateContactInfoDto
                {
                    Email = "Email",
                    PhoneNumber = "1234567890",
                }
            };

            // Assert
            Assert.Throws<InvalidModelException>(() => _service.CreateWithDependencies(dto));
        }

        [Test]
        [Category("CreateWithDependencies")]
        //[Ignore("Transaction are not supported in-memory databases")]
        public void CreateWithDependencies_ContactInfoInDtoIsNull_ThrowInvalidModelException()
        {
            Tools.IgnoreInMemoryDatabase();

            // Arrange
            CreateCompanyWithDependenciesDto dto = new CreateCompanyWithDependenciesDto
            {
                Name = "CompanyName",

                Address = new CreateAddressDto
                {
                    BuildingNumber = "BuildingNumber",
                    City = "City",
                    Country = "Country",
                    Street = "Street",
                },

                ContactInfo = null
            };

            // Assert
            Assert.Throws<InvalidModelException>(() => _service.CreateWithDependencies(dto));
        }

        [Test]
        [Category("CreateWithDependencies")]
        //[Ignore("Transaction are not supported in-memory databases")]
        public void CreateWithDependencies_ContactInfoInDtoIsNullAndAddressInDtoIsNull_ThrowInvalidModelException()
        {
            Tools.IgnoreInMemoryDatabase();

            // Arrange
            CreateCompanyWithDependenciesDto dto = new CreateCompanyWithDependenciesDto
            {
                Name = "CompanyName",
                Address = null,
                ContactInfo = null
            };

            // Assert
            Assert.Throws<InvalidModelException>(() => _service.CreateWithDependencies(dto));
        }

        [Test]
        [Category("CreateWithDependencies")]
        //[Ignore("Transaction are not supported in-memory databases")]
        public void CreateWithDependencies_DtoIsValid_ReturnDto()
        {
            Tools.IgnoreInMemoryDatabase();

            // Arrange
            CreateCompanyWithDependenciesDto dto = new CreateCompanyWithDependenciesDto
            {
                Name = "CompanyName",

                Address = new CreateAddressDto
                {
                    BuildingNumber = "BuildingNumber",
                    City = "City",
                    Country = "Country",
                    Street = "Street",
                },

                ContactInfo = new CreateContactInfoDto
                {
                    Email = "Email",
                    PhoneNumber = "1234567890",
                }
            };

            // Act
            var result = _service.CreateWithDependencies(dto);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.NotNull(result);
                Assert.IsTrue(Tools.ModelIsValid(result));
            });
        }

        // ReadWithDependencies
        [Test]
        [Category("ReadWithDependencies")]
        public void ReadWithDependencies_IdNotInDb_ThrowNotFoundException()
        {
            // Arrange
            int searchedId = -1;

            // Assert
            Assert.Throws<NotFoundException>(() => _service.ReadWithDependencies(searchedId));
        }

        [Test]
        [Category("ReadWithDependencies")]
        public void ReadWithDependencies_IdNotInDb_ReturnGetCompanyWithDependenciesDto()
        {
            // Arrange
            Company company = Tools.GetExampleCompanyWithDependecies(_dbContext);
            _dbContext.Companys.Add(company);
            _dbContext.SaveChanges();

            // Act
            GetCompanyWithDependenciesDto result = _service.ReadWithDependencies(company.Id);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.True(Tools.ModelIsValid(result));
            });
        }

        // ReadAllWithDependencies
        [Test]
        [Category("ReadAllWithDependencies")]
        public void ReadAllWithDependencies_NoRecordsInDb_ReturnEmptyArray()
        {
            // Assert
            int numberOfRecordsInDb = _dbContext.Addresses.ToArray().Length;

            // Act
            GetCompanyWithDependenciesDto[] result = _service.ReadAllWithDependencies();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Length, Is.EqualTo(numberOfRecordsInDb));
                Assert.That(numberOfRecordsInDb, Is.Zero);
            });
        }

        [Test]
        [Category("ReadAllWithDependencies")]
        [TestCase(3)]
        public void ReadAllWithDependencies_RecordsInDb_ReturnDtoArray(int howManyToAdd)
        {
            // Arrange
            for (int i = 0; i < howManyToAdd; i++)
            {
                Company company = Tools.GetExampleCompanyWithDependecies(_dbContext);
                _dbContext.Companys.Add(company);
            }
            _dbContext.SaveChanges();
            int howManyRecordsInDb = _dbContext.Companys.ToArray().Length;

            // Act
            GetCompanyWithDependenciesDto[] result = _service.ReadAllWithDependencies();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Length, Is.EqualTo(howManyToAdd));
                Assert.That(result.Length, Is.EqualTo(howManyRecordsInDb));
            });
        }
    }
}