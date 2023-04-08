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

        public ProductService(LBT_DbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public GetProductDto Create(CreateProductDto dto)
        {
            // Check dto
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            // Check dto fields
            bool dtoIsValid = Tools.AllStringPropsAreNotNull(dto);
            if (dtoIsValid == false)
                throw new BadRequestException("Dto is missing fields");

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

            // Return GetProductDto
            GetProductDto outputDto = _mapper.Map<GetProductDto>(product);

            return outputDto;
        }

        public GetProductDto[] ReadAll()
        {
            Product[] products = _dbContext.Products.ToArray();
            GetProductDto[] outputDtos = _mapper.Map<GetProductDto[]>(products);

            return outputDtos;
        }

        public GetProductDto Update(UpdateProductDto dto)
        {
            // Check dto
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (dto.Id == null)
                throw new BadRequestException("Dto is missing Id field");

            // Check if record exist
            Product? product = _dbContext.Products.FirstOrDefault(a => a.Id == dto.Id);
            if (product == null)
                throw new NotFoundException("Product with Id: " + dto.Id);

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
