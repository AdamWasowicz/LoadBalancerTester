using AutoMapper;
using LBT_Api.Entities;
using LBT_Api.Exceptions;
using LBT_Api.Interfaces.Services;
using LBT_Api.Models.AddressDto;
using LBT_Api.Models.ProductDto;
using LBT_Api.Models.SupplierDto;
using LBT_Api.Other;
using LBT_Api.Services;
using Microsoft.EntityFrameworkCore;


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
            // Set up db
            _dbContext = Tools.GetDbContext();

            // AutoMapper
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<LBT_Entity_MappingProfile>());
            IMapper mapper = mapperConfig.CreateMapper();

            // Dependencies
            ISupplierService supplierService = new SupplierService(_dbContext, mapper);

            // Service
            _service = new ProductService(_dbContext, mapper, supplierService);
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
            CreateProductDto dto = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => _service.Create(dto));
        }

        [Test]
        [Category("Create")]
        public void Create_DtoHasMissingFields_ThrowInvalidModelException()
        {
            // Arrange
            CreateProductDto dto = new CreateProductDto();

            // Assert
            Assert.Throws<InvalidModelException>(() => _service.Create(dto));
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

        // CreateWithDependencies
        [Test]
        [Category("CreateWithDependencies")]
        //[Ignore("Transaction are not supported in-memory databases")]
        public void CreateWithDependencies_DtoIsNull_ThrowArgumentNullException()
        {
            Tools.IgnoreInMemoryDatabase();

            // Arrange
            CreateProductWithDependenciesDto dto = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => _service.CreateWithDependencies(dto));
        }

        [Test]
        [Category("CreateWithDependencies")]
        //[Ignore("Transaction are not supported in-memory databases")]
        public void CreateWithDependencies_SupplierInDtoIsNull_ThrowInvalidModelException()
        {
            Tools.IgnoreInMemoryDatabase();

            // Arrange
            CreateProductWithDependenciesDto dto = new CreateProductWithDependenciesDto()
            {
                Name = "Name",
                PriceNow = 10,
                Supplier = null,
            };

            // Assert
            Assert.Throws<InvalidModelException>(() => _service.CreateWithDependencies(dto));
        }

        [Test]
        [Category("CreateWithDependencies")]
        //[Ignore("Transaction are not supported in-memory databases")]
        public void CreateWithDependencies_DtoModelIsInvalid_ThrowInvalidModelException()
        {
            Tools.IgnoreInMemoryDatabase();

            // Arrange
            CreateProductWithDependenciesDto dto = new CreateProductWithDependenciesDto()
            {
                Name = null,
                PriceNow = 10,
                Supplier = new CreateSupplierWithDependenciesDto
                {
                    Name = "Name",
                    Address = new CreateAddressDto
                    {
                        Country = "Country",
                        City = "City",
                        Street = "Street",
                        BuildingNumber = "BuildingNumber"
                    }
                }
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
            CreateProductWithDependenciesDto dto = new CreateProductWithDependenciesDto()
            {
                Name = "Name",
                PriceNow = 10,
                Supplier = new CreateSupplierWithDependenciesDto
                {
                    Name = "Name",
                    Address = new CreateAddressDto
                    {
                        Country = "Country",
                        City = "City",
                        Street = "Street",
                        BuildingNumber = "BuildingNumber"
                    }
                }
            };

            // Act
            GetProductWithDependenciesDto result = _service.CreateWithDependencies(dto);

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
            Product product = Tools.GetExampleProductWithDependencies(_dbContext);
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();

            int numberOfRecordsBeforeOperation = _dbContext.Products.Count();

            // Act
            _service.Delete(product.Id);

            // Assert
            int numberOfRecordsAfterOperation = _dbContext.Products.Count();

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
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Tools.AssertObjectsAreSameAsJSON(result, productAsDto);
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
        public void ReadWithDependencies_IdInDb_ReturnDto()
        {
            // Arrang
            Product product = Tools.GetExampleProductWithDependencies(_dbContext);
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();

            // Act
            GetProductWithDependenciesDto result = _service.ReadWithDependencies(product.Id);

            // Assert
            bool rowExistsInDb = _dbContext.Products.Any(p => p.Id == product.Id);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(result);
                Assert.IsTrue(Tools.ModelIsValid(result));
                Assert.IsTrue(rowExistsInDb);
            });
        }

        // ReadAll
        [Test]
        [Category("ReadAll")]
        public void ReadAll_NoRecordsInDb_ReturnEmptyArray()
        {
            // Arrange
            int numberOfRecordsInDb = _dbContext.Products.Count();

            // Act
            GetProductDto[] result = _service.ReadAll();

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
                Product product = Tools.GetExampleProductWithDependencies(_dbContext);
                _dbContext.Products.Add(product);
                _dbContext.SaveChanges();
            }
            int howManyRecordsInDb = _dbContext.Products.Count();

            // Act
            GetProductDto[] result = _service.ReadAll();

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
            // Arrange
            int numberOfRecordsInDb = _dbContext.Products.Count();

            // Act
            GetProductWithDependenciesDto[] result = _service.ReadAllWithDependencies();

            // Assert
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
                Product product = Tools.GetExampleProductWithDependencies(_dbContext);
                _dbContext.Products.Add(product);
                _dbContext.SaveChanges();
            }
            int howManyRecordsInDb = _dbContext.Products.Count();

            // Act
            GetProductWithDependenciesDto[] result = _service.ReadAllWithDependencies();

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
            UpdateProductDto dto = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => _service.Update(dto));
        }

        [Test]
        [Category("Update")]
        public void Update_DtoIsMissingId_ThrowInvalidModelException()
        {
            // Arrange
            UpdateProductDto dto = new UpdateProductDto();

            // Assert
            Assert.Throws<InvalidModelException>(() => _service.Update(dto));
        }

        [Test]
        [Category("Update")]
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
        [Category("Update")]
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
