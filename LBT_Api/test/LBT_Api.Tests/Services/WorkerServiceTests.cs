using AutoMapper;
using LBT_Api.Entities;
using LBT_Api.Exceptions;
using LBT_Api.Interfaces.Services;
using LBT_Api.Models.AddressDto;
using LBT_Api.Models.CompanyDto;
using LBT_Api.Models.ContactInfoDto;
using LBT_Api.Models.WorkerDto;
using LBT_Api.Other;
using LBT_Api.Services;
using Microsoft.EntityFrameworkCore;

namespace LBT_Api.Tests.Services
{
    /// <summary>
    /// Unit tests for WorkerService implementation of IWorkerService interface
    /// </summary>
    [TestFixture]
    public class WorkerServiceTests
    {
        private LBT_DbContext _dbContext;
        private IWorkerService _service;

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

            // Dependencies 
            ICompanyService companyService = new CompanyService(_dbContext, mapper);

            // Service
            _service = new WorkerService(_dbContext, mapper, companyService);
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
            CreateWorkerDto dto = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => _service.Create(dto));
        }

        [Test]
        [Category("Create")]
        public void Create_DtoHasMissingFields_ThrowInvalidModelException()
        {
            // Arrange
            CreateWorkerDto dto = new CreateWorkerDto();

            // Assert
            Assert.Throws<InvalidModelException>(() => _service.Create(dto));
        }

        [Test]
        [Category("Create")]
        public void Create_DtoIsValid_ReturnDto()
        {
            // Arrange
            Company company = Tools.GetExampleCompanyWithDependecies(_dbContext);
            _dbContext.Companys.Add(company);
            _dbContext.SaveChanges();

            Address address = Tools.GetExampleAddress();
            _dbContext.Addresses.Add(address);
            _dbContext.SaveChanges();

            ContactInfo ci = Tools.GetExampleContactInfo();
            _dbContext.ContactInfos.Add(ci);
            _dbContext.SaveChanges();

            CreateWorkerDto dto = new CreateWorkerDto()
            {
                CompanyId = company.Id,
                AddressId = address.Id,
                ContactInfoId = ci.Id,
                Name = "Name",
                Surname = "Surname"
            };

            // Act
            GetWorkerDto result = _service.Create(dto);

            // Assert
            Assert.That(result, Is.Not.Null);   
        }

        // CreateWithDependencies
        [Test]
        [Category("CreateWithDependencies")]
        //[Ignore("Transaction are not supported in-memory databases")]
        public void CreateWithDependencies_DtoIsNull_ThrowArgumenNullException()
        {
            Tools.IgnoreInMemoryDatabase();

            // Arrange
            CreateWorkerWithDependenciesDto dto = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => _service.CreateWithDependencies(dto));
        }

        [Test]
        [Category("CreateWithDependencies")]
        //[Ignore("Transaction are not supported in-memory databases")]
        public void CreateWithDependencies_DtoIsMissingFields_ThrowInvalidException()
        {
            Tools.IgnoreInMemoryDatabase();

            // Arrange
            CreateWorkerWithDependenciesDto dto = new CreateWorkerWithDependenciesDto
            {
                Name = "Name",
                Surname = "Surname",
                Comapny = null,
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
            CreateAddressDto address = new CreateAddressDto
            {
                Country = "Country",
                City = "City",
                Street = "Street",
                BuildingNumber = "BuildingNumber"
            };

            CreateContactInfoDto ci = new CreateContactInfoDto
            {
                Email = "Email",
                PhoneNumber = "PhoneNumber"
            };

            CreateWorkerWithDependenciesDto dto = new CreateWorkerWithDependenciesDto
            {
                Name = "Name",
                Surname = "Surname",
                Comapny = new CreateCompanyWithDependenciesDto
                {
                    Name = "Name",
                    Address = address,
                    ContactInfo = ci
                },
                Address = address,
                ContactInfo = ci
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
        public void Delete_IdInDb_ReturnZero()
        {
            // Arrange
            Worker worker = Tools.GetExampleWorkerWithDependencies(_dbContext);
            _dbContext.Workers.Add(worker);
            _dbContext.SaveChanges();

            int numberOfRecordsBeforeOperation = _dbContext.Workers.Count();

            // Act
            var result = _service.Delete(worker.Id);

            //Assert
            int numberOfRecordsAfterOperation = _dbContext.Workers.Count();

            Assert.That(result, Is.EqualTo(0));
            Assert.That(numberOfRecordsAfterOperation, Is.EqualTo(0));
            Assert.That(numberOfRecordsBeforeOperation, Is.GreaterThan(numberOfRecordsAfterOperation));
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
            Worker worker = Tools.GetExampleWorkerWithDependencies(_dbContext);
            _dbContext.Workers.Add(worker);
            _dbContext.SaveChanges();

            // Act
            GetWorkerDto result = _service.Read(worker.Id);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(worker.Id, Is.EqualTo(result.Id));
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
            Worker worker = Tools.GetExampleWorkerWithDependencies(_dbContext);
            _dbContext.Workers.Add(worker);
            _dbContext.SaveChanges();

            // Act
            GetWorkerWithDependenciesDto result = _service.ReadWithDependencies(worker.Id);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.True(Tools.ModelIsValid(result));
            });
        }

        // ReadAll
        [Test]
        [Category("ReadAll")]
        public void ReadAll_NoRecordsInDb_ReturnEmptyArray()
        {
            // Assert
            int numberOfRecordsInDb = _dbContext.Workers.ToArray().Length;

            // Act
            GetWorkerDto[] result = _service.ReadAll();

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
                Worker worker = Tools.GetExampleWorkerWithDependencies(_dbContext);
                _dbContext.Workers.Add(worker);
                _dbContext.SaveChanges();
            }
            int howManyRecordsInDb = _dbContext.Workers.Count();

            // Act
            GetWorkerDto[] result = _service.ReadAll();

            // Assert
            Assert.That(result.Length, Is.EqualTo(howManyToAdd));
            Assert.That(result.Length, Is.EqualTo(howManyRecordsInDb));
        }

        // ReadAllWithDependencies
        [Test]
        [Category("ReadAllWithDependencies")]
        public void ReadAllWithDependencies_NoRecordsInDb_ReturnEmptyArray()
        {
            // Assert
            int numberOfRecordsInDb = _dbContext.Workers.ToArray().Length;

            // Act
            GetWorkerWithDependenciesDto[] result = _service.ReadAllWithDependencies();

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
            for (int i = 0; i < howManyToAdd; i++)
            {
                Worker worker = Tools.GetExampleWorkerWithDependencies(_dbContext);
                _dbContext.Workers.Add(worker);
            }
            _dbContext.SaveChanges();
            int howManyRecordsInDb = _dbContext.Workers.ToArray().Length;

            // Act
            GetWorkerWithDependenciesDto[] result = _service.ReadAllWithDependencies();

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
            UpdateWorkerDto dto = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => _service.Update(dto));
        }

        [Test]
        [Category("Update")]
        public void Update_DtoIsMissingId_ThrowBadRequestException()
        {
            // Arrange
            UpdateWorkerDto dto = new UpdateWorkerDto();

            // Assert
            Assert.Throws<BadRequestException>(() => _service.Update(dto));
        }

        [Test]
        [Category("Update")]
        public void Update_IdFromDtoNotInDb_ThrowNotFoundException()
        {
            // Arrange
            UpdateWorkerDto dto = new UpdateWorkerDto()
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
            Worker worker = Tools.GetExampleWorkerWithDependencies(_dbContext);
            _dbContext.Workers.Add(worker);
            _dbContext.SaveChanges();

            UpdateWorkerDto dto = new UpdateWorkerDto()
            {
                Id = worker.Id,
                Name = worker.Name + "Upated",
                Surname = worker.Surname + "Updated"
            };

            // Act
            GetWorkerDto result = _service.Update(dto);

            // Assert
            Assert.That(result.Name, Is.EqualTo(dto.Name));
            Assert.That(result.Surname, Is.EqualTo(dto.Surname));
        }
    }
}
