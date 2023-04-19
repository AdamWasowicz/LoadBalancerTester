using AutoMapper;
using LBT_Api.Entities;
using LBT_Api.Exceptions;
using LBT_Api.Interfaces.Services;
using LBT_Api.Models.AddressDto;
using LBT_Api.Models.CompanyDto;
using LBT_Api.Models.ContactInfoDto;
using LBT_Api.Models.ProductSoldDto;
using LBT_Api.Models.SaleDto;
using LBT_Api.Models.WorkerDto;
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
            // Set up db
            _dbContext = Tools.GetDbContext();

            // AutoMapper
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<LBT_Entity_MappingProfile>());
            IMapper mapper = mapperConfig.CreateMapper();

            // Dependencies
            ICompanyService companyService = new CompanyService(_dbContext, mapper);
            IWorkerService workerService = new WorkerService(_dbContext, mapper, companyService);

            // Service
            _service = new SaleService(_dbContext, mapper, workerService);
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
            CreateSaleDto dto = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => _service.Create(dto));
        }

        [Test]
        [Category("Create")]
        public void Create_DtoHasMissingFields_ThrowInvalidModelException()
        {
            // Arrange
            CreateSaleDto dto = new CreateSaleDto();

            // Assert
            Assert.Throws<InvalidModelException>(() => _service.Create(dto));
        }

        [Test]
        [Category("Create")]
        public void Create_DtoIsValid_ReturnDto()
        {
            // Arrange
            Worker worker = Tools.GetExampleWorkerWithDependencies(_dbContext);
            _dbContext.Workers.Add(worker);

            Product product = Tools.GetExampleProductWithDependencies(_dbContext);
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();
            
            CreateSaleDto dto = new CreateSaleDto()
            {
                WorkerId = worker.Id,
            };

            int amountOfRowsBeforeAction = _dbContext.Sales.Count();

            // Act
            GetSaleDto result = _service.Create(dto);

            // Assert
            int amountOfRowsAfterAction = _dbContext.Sales.Count();

            Assert.Multiple(() =>
            {
                Assert.Greater(amountOfRowsAfterAction, amountOfRowsBeforeAction);
                Assert.That(result.SumValue, Is.EqualTo(0));
            });
        }

        // CreateWithDependencies
        [Test]
        [Category("CreateWithDependencies")]
        //[Ignore("Transaction are not supported in-memory databases")]
        public void CreateWithDependencies_DtoIsNull_ThrowArgumenNullException()
        {
            Tools.IgnoreInMemoryDatabase();

            // Arrange
            CreateSaleWithDependenciesDto dto = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => _service.CreateWithDependencies(dto));
        }

        [Test]
        [Category("CreateWithDependencies")]
        //[Ignore("Transaction are not supported in-memory databases")]
        public void CreateWithDependencies_DtoHasMissingFields_ThrowInvalidModelException()
        {
            Tools.IgnoreInMemoryDatabase();

            // Arrange
            CreateSaleWithDependenciesDto dto = new CreateSaleWithDependenciesDto();

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
            CreateSaleWithDependenciesDto dto = new CreateSaleWithDependenciesDto
            {
                Worker = new CreateWorkerWithDependenciesDto
                {
                    Name = "Name",
                    Surname = "Surname",
                    Address = new CreateAddressDto
                    {
                        Country = "Country",
                        City = "City",
                        BuildingNumber = "BN",
                        Street = "Street"
                    },// Address
                    ContactInfo = new CreateContactInfoDto
                    {
                        Email = "Email",
                        PhoneNumber = "PhoneNumber"
                    },// ContactInfo
                    Comapny = new CreateCompanyWithDependenciesDto
                    {
                        Name = "Name",
                        Address = new CreateAddressDto
                        {
                            Country = "Country",
                            City = "City",
                            BuildingNumber = "BN",
                            Street = "Street"
                        },// Address
                        ContactInfo = new CreateContactInfoDto
                        {
                            Email = "Email",
                            PhoneNumber = "PhoneNumber"
                        },// ContactInfo
                    }// Company
                },// Worker
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
        //[Ignore("Transaction are not supported in-memory databases")]
        public void Delete_IdInDb_Return()
        {
            Tools.IgnoreInMemoryDatabase();

            // Arrange
            Sale sale = Tools.GetExampleSaleWithDependencies(_dbContext);
            _dbContext.Sales.Add(sale);
            _dbContext.SaveChanges();

            int numberOfRecordsBeforeOperation = _dbContext.Sales.Count();

            // Act
            _service.Delete(sale.Id);

            // Assert
            int numberOfRecordsAfterOperation = _dbContext.Sales.Count();

            Assert.Multiple(() =>
            {
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
            Sale sale = Tools.GetExampleSaleWithDependencies(_dbContext);
            _dbContext.Sales.Add(sale);
            _dbContext.SaveChanges();

            // Act
            GetSaleDto result = _service.Read(sale.Id);

            // Assert
            Assert.That(result, Is.Not.Null);
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
            Sale sale = Tools.GetExampleSaleWithDependencies(_dbContext);
            _dbContext.Sales.Add(sale);
            _dbContext.SaveChanges();

            // Act
            GetSaleWithDependenciesDto result = _service.ReadWithDependencies(sale.Id);

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
            // Arrange
            int numberOfRecordsInDb = _dbContext.Sales.ToArray().Length;

            // Act
            GetSaleDto[] result = _service.ReadAll();

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
                Sale sale = Tools.GetExampleSaleWithDependencies(_dbContext);
                _dbContext.Sales.Add(sale);
            }
            _dbContext.SaveChanges();
            int howManyRecordsInDb = _dbContext.Sales.ToArray().Length;

            // Act
            GetSaleDto[] result = _service.ReadAll();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Length, Is.EqualTo(howManyToAdd));
                Assert.That(result.Length, Is.EqualTo(howManyRecordsInDb));
            });
        }

        // ReadAllWithDependencies
        [Test]
        [Category("ReadAllWithDependencies")]
        public void ReadAllWithDependencies_NoRecordsInDb_ReturnEmptyArray()
        {
            // Assert
            int numberOfRecordsInDb = _dbContext.Sales.ToArray().Length;

            // Act
            GetSaleWithDependenciesDto[] result = _service.ReadAllWithDependencies();

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
                Sale sale = Tools.GetExampleSaleWithDependencies(_dbContext);
                _dbContext.Sales.Add(sale);
            }
            _dbContext.SaveChanges();
            int howManyRecordsInDb = _dbContext.Sales.ToArray().Length;

            // Act
            GetSaleWithDependenciesDto[] result = _service.ReadAllWithDependencies();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Length, Is.EqualTo(howManyToAdd));
                Assert.That(result.Length, Is.EqualTo(howManyRecordsInDb));
            });
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
        public void Update_DtoIsMissingId_ThrowInvalidModelException()
        {
            // Arrange
            UpdateSaleDto dto = new UpdateSaleDto();

            // Assert
            Assert.Throws<InvalidModelException>(() => _service.Update(dto));
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
