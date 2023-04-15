using AutoMapper;
using LBT_Api.Entities;
using LBT_Api.Exceptions;
using LBT_Api.Interfaces.Services;
using LBT_Api.Models.AddressDto;
using LBT_Api.Models.CompanyDto;
using LBT_Api.Models.ContactInfoDto;
using LBT_Api.Models.ProductDto;
using LBT_Api.Models.ProductSoldDto;
using LBT_Api.Models.SaleDto;
using LBT_Api.Models.SupplierDto;
using LBT_Api.Models.WorkerDto;
using LBT_Api.Other;
using LBT_Api.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LBT_Api.Tests.Services
{
    /// <summary>
    /// Unit tests for ProductSold implementation of IProductSold interface
    /// </summary>
    [TestFixture]
    public class ProductSoldServiceTests
    {
        private LBT_DbContext _dbContext;
        private IProductSoldService _service;

        [SetUp]
        public void Setup()
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
            ISupplierService supplierService = new SupplierService(_dbContext, mapper);
            IProductService productService = new ProductService(_dbContext, mapper, supplierService);
            ISaleService saleService = new SaleService(_dbContext, mapper);

            // Service
            _service = new ProductSoldService(_dbContext, mapper, productService, saleService);
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
            CreateProductSoldDto dto = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => _service.Create(dto));
        }

        [Test]
        [Category("Create")]
        public void Create_DtoHasMissingFields_ThrowInvalidModelException()
        {
            // Arrange
            CreateProductSoldDto dto = new CreateProductSoldDto();

            // Assert
            Assert.Throws<InvalidModelException>(() => _service.Create(dto));
        }

        [Test]
        [Category("Create")]
        public void Create_DtoIsValid_ReturnDto()
        {
            // Arrange
            Product product = Tools.GetExampleProductWithDependencies(_dbContext);
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();

            Sale sale = Tools.GetExampleSaleWithDependencies(_dbContext);
            _dbContext.Sales.Add(sale);
            _dbContext.SaveChanges();

            CreateProductSoldDto dto = new CreateProductSoldDto()
            {
                AmountSold = 1,
                ProductId = product.Id,
                SaleId = sale.Id
            };

            // Act
            GetProductSoldDto result = _service.Create(dto);

            // Assert
            double saleSumValue = _dbContext.Sales.FirstOrDefault(s => s.Id == sale.Id).SumValue;
            double expectedSumValue = _dbContext.ProductsSold.Where(ps => ps.SaleId == sale.Id).Sum(ps => ps.AmountSold * ps.PriceAtTheTimeOfSale);
            int amountOfRowsInDb = _dbContext.ProductsSold.Count();

            Assert.That(saleSumValue, Is.EqualTo(expectedSumValue));
            Assert.That(amountOfRowsInDb, Is.EqualTo(1));
        }

        // CreateWithDependencies
        [Test]
        [Category("CreateWithDependencies")]
        //[Ignore("Transaction are not supported in-memory databases")]
        public void CreateWithDependencies_DtoIsNull_ThrowArgumenNullException()
        {
            Tools.IgnoreInMemoryDatabase();

            // Arrange
            CreateProductSoldWithDependenciesDto dto = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => _service.CreateWithDependencies(dto));
        }

        [Test]
        [Category("CreateWithDependencies")]
        //[Ignore("Transaction are not supported in-memory databases")]
        public void CreateWithDependencies_DtoIsMissingFields_ThrowInvalidModelException()
        {
            Tools.IgnoreInMemoryDatabase();

            // Arrange
            CreateProductSoldWithDependenciesDto dto = new CreateProductSoldWithDependenciesDto
            {
                AmountSold = 2,
                Product = null,
                Sale = null
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
            CreateProductSoldWithDependenciesDto dto = new CreateProductSoldWithDependenciesDto
            {
                AmountSold = 2,
                Product = new CreateProductWithDependenciesDto
                {
                    Name = "Product",
                    PriceNow = 5.55,
                    Supplier = new CreateSupplierWithDependenciesDto
                    {
                        Name = "Supplier",
                        Address = new CreateAddressDto
                        {
                            City = "City",
                            Country = "Country",
                            BuildingNumber = "BN",
                            Street = "Street"
                        }// Address
                    }// Supplier
                },// Product
                Sale = new CreateSaleWithDependenciesDto
                {
                    Worker = new CreateWorkerWithDependenciesDto
                    {
                        Name = "Name",
                        Surname = "Surname",
                        Address = new CreateAddressDto
                        {
                            City = "City",
                            Country = "Country",
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
                            Name = "Company",
                            Address = new CreateAddressDto
                            {
                                City = "City",
                                Country = "Country",
                                BuildingNumber = "BN",
                                Street = "Street"
                            },// Address
                            ContactInfo = new CreateContactInfoDto
                            {
                                Email = "Email",
                                PhoneNumber = "PhoneNumber"
                            }// ContactInfo
                        }// Company
                    }// Worker
                }// Sale
            };// ProductSold

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
            ProductSold ps = Tools.GetExampleProductSoldWithDependencies(_dbContext);
            _dbContext.ProductsSold.Add(ps);
            _dbContext.SaveChanges();

            // Act
            int result = _service.Delete(ps.Id);
            int numberOfRecordsAfterOperation = _dbContext.ProductsSold.ToArray().Length;

            // Assert
            Assert.That(result, Is.EqualTo(0));
            Assert.That(numberOfRecordsAfterOperation, Is.EqualTo(0));
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
            ProductSold ps = Tools.GetExampleProductSoldWithDependencies(_dbContext);
            _dbContext.ProductsSold.Add(ps);
            _dbContext.SaveChanges();

            // Act
            GetProductSoldDto result = _service.Read(ps.Id);

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
            ProductSold ps = Tools.GetExampleProductSoldWithDependencies(_dbContext);
            _dbContext.ProductsSold.Add(ps);
            _dbContext.SaveChanges();

            // Act
            GetProductSoldWithDependenciesDto result = _service.ReadWithDependencies(ps.Id);

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
            int numberOfRecordsInDb = _dbContext.Addresses.ToArray().Length;

            // Act
            GetProductSoldDto[] result = _service.ReadAll();

            // Arrange
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
                ProductSold ps = Tools.GetExampleProductSoldWithDependencies(_dbContext);
                _dbContext.ProductsSold.Add(ps);
                _dbContext.SaveChanges();
            }
            int howManyRecordsInDb = _dbContext.ProductsSold.ToArray().Length;

            // Act
            GetProductSoldDto[] result = _service.ReadAll();

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
            int numberOfRecordsInDb = _dbContext.Addresses.ToArray().Length;

            // Act
            GetProductSoldWithDependenciesDto[] result = _service.ReadAllWithDependencies();

            // Arrange
            Assert.Multiple(() =>
            {
                Assert.That(result.Length, Is.EqualTo(numberOfRecordsInDb));
                Assert.That(result.Length, Is.EqualTo(0));
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
                ProductSold ps = Tools.GetExampleProductSoldWithDependencies(_dbContext);
                _dbContext.ProductsSold.Add(ps);
            }
            _dbContext.SaveChanges();
            int howManyRecordsInDb = _dbContext.ProductsSold.ToArray().Length;

            // Act
            GetProductSoldWithDependenciesDto[] result = _service.ReadAllWithDependencies();

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
            UpdateProductSoldPrice dto = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => _service.Update(dto));
        }

        [Test]
        [Category("Update")]
        public void Update_DtoIsMissingId_ThrowInvalidModelException()
        {
            // Arrange
            UpdateProductSoldPrice dto = new UpdateProductSoldPrice();

            // Assert
            Assert.Throws<InvalidModelException>(() => _service.Update(dto));
        }

        [Test]
        [Category("Update")]
        public void Update_IdFromDtoNotInDb_ThrowNotFoundException()
        {
            // Arrange
            UpdateProductSoldPrice dto = new UpdateProductSoldPrice()
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
            ProductSold ps = Tools.GetExampleProductSoldWithDependencies(_dbContext);
            _dbContext.ProductsSold.Add(ps);
            _dbContext.SaveChanges();

            UpdateProductSoldPrice dto = new UpdateProductSoldPrice()
            {
                Id = ps.Id,
                AmountSold = ps.AmountSold + 1,
                PriceAtTheTimeOfSale = ps.PriceAtTheTimeOfSale + 1,
                
            };

            // Act
            GetProductSoldDto result = _service.Update(dto);

            var s = _dbContext.Sales.ToArray();

            // Assert
            double saleSumValue = _dbContext.Sales.FirstOrDefault(s => s.Id == ps.SaleId).SumValue;
            double expectedSumValue = _dbContext.ProductsSold.Where(ps => ps.SaleId == ps.SaleId).Sum(ps => ps.AmountSold * ps.PriceAtTheTimeOfSale);
            int amountOfRowsInDb = _dbContext.ProductsSold.Count();

            Assert.That(saleSumValue, Is.EqualTo(expectedSumValue));
            Assert.That(amountOfRowsInDb, Is.EqualTo(1));
        }
    }
}
