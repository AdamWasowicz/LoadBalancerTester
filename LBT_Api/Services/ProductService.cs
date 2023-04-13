using AutoMapper;
using LBT_Api.Entities;
using LBT_Api.Exceptions;
using LBT_Api.Interfaces.Services;
using LBT_Api.Models.ProductDto;
using LBT_Api.Other;

namespace LBT_Api.Services
{
    public class ProductService : IProductService
    {
        private readonly LBT_DbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ISupplierService _supplierService;

        public ProductService(LBT_DbContext dbContext, IMapper mapper, ISupplierService supplierService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _supplierService = supplierService;
        }

        public GetProductDto Create(CreateProductDto dto)
        {
            // Check dto
            if (Tools.ModelIsValid(dto) == false)
                throw new InvalidModelException("Dto is missing fields");

            // Create record
            Product product = _mapper.Map<Product>(dto);
            try
            {
                _dbContext.Products.Add(product);
                _dbContext.SaveChanges();
            }
            catch (Exception exception) 
            {
                throw new DatabaseOperationFailedException(exception.Message);
            }

            GetProductDto outputDto = _mapper.Map<GetProductDto>(product);

            return outputDto;
        }

        public GetProductWithDependenciesDto CreateWithDependencies(CreateProductWithDependenciesDto dto)
        {
            // Check dto
            if (Tools.ModelIsValid(dto) == false)
                throw new InvalidModelException("Dto is missing fields");

            var transaction = _dbContext.Database.BeginTransaction();
            Product product = null;

            try
            {
                // Dependencies
                int supplierId = _supplierService.CreateWithDependencies(dto.Supplier).Id;

                // Main
                product = new Product()
                {
                    Name = dto.Name,
                    PriceNow = dto.PriceNow,
                    SupplierId = supplierId,
                };
                _dbContext.Products.Add(product);

                // Save changes
                _dbContext.SaveChanges();
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                throw new DatabaseOperationFailedException(exception.Message);
            }

            // Return Dto
            GetProductWithDependenciesDto outputDto = _mapper.Map<GetProductWithDependenciesDto>(product);

            return outputDto;
        }

        public int Delete(int id)
        {
            // Check if record exists
            Product? product = _dbContext.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                throw new NotFoundException("Product with Id: " + id);

            // Delete record
            try
            {
                _dbContext.Products.Remove(product);
                _dbContext.SaveChanges();
            }
            catch (Exception exception)
            {
                throw new DatabaseOperationFailedException(exception.Message);
            }

            return 0;
        }

        public GetProductDto Read(int id)
        {
            // Check if record exists
            Product? product = _dbContext.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                throw new NotFoundException("Product with Id: " + id);

            // Return Dto
            GetProductDto outputDto = _mapper.Map<GetProductDto>(product);

            return outputDto;
        }

        public GetProductWithDependenciesDto ReadWithDependencies(int id)
        {
            // Check if record exists
            Product? product = _dbContext.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                throw new NotFoundException("Product with Id: " + id);

            // Return Dto
            GetProductWithDependenciesDto outputDto = _mapper.Map<GetProductWithDependenciesDto>(product);

            return outputDto;
        }

        public GetProductDto[] ReadAll()
        {
            Product[] products = _dbContext.Products.ToArray();
            GetProductDto[] outputDto = _mapper.Map<GetProductDto[]>(products);

            return outputDto;
        }

        public GetProductWithDependenciesDto[] ReadAllWithDependencies()
        {
            Product[] products = _dbContext.Products.ToArray();
            GetProductWithDependenciesDto[] outputDto = _mapper.Map<GetProductWithDependenciesDto[]>(products);

            return outputDto;
        }

        public GetProductDto Update(UpdateProductDto dto)
        {
            // Check dto
            if (Tools.ModelIsValid(dto) == false)
                throw new InvalidModelException("Dto is missing fields");

            // Check if record exist
            Product? product = _dbContext.Products.FirstOrDefault(a => a.Id == dto.Id);
            if (product == null)
                throw new NotFoundException("Product with Id: " + dto.Id);

            // Make changes
            Product mappedProductFromDto = _mapper.Map<Product>(dto);
            product = Tools.UpdateObjectProperties(product, mappedProductFromDto);

            // Save changes
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception exception)
            {
                throw new DatabaseOperationFailedException(exception.Message);
            }

            GetProductDto outputDto = _mapper.Map<GetProductDto>(product);

            return outputDto;
        }
    }
}
