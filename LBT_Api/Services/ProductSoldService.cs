using AutoMapper;
using LBT_Api.Entities;
using LBT_Api.Exceptions;
using LBT_Api.Interfaces.Services;
using LBT_Api.Models.ProductSoldDto;
using LBT_Api.Other;
using System.Net;

namespace LBT_Api.Services
{
    public class ProductSoldService : IProductSoldService
    {
        private readonly LBT_DbContext _dbContext;
        private readonly IMapper _mapper;

        public ProductSoldService(LBT_DbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public GetProductSoldDto Create(CreateProductSold_SoloDto dto)
        {
            // Check dto
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            // Check dto fields
            bool dtoIsValid = Tools.AllStringPropsAreNotNull(dto);
            if (dtoIsValid == false || dto.SaleId == null || dto.ProductId == null)
                throw new BadRequestException("Dto is missing fields");

            // Check if dependencies exist
            Sale? sale = _dbContext.Sales.FirstOrDefault(s => s.Id == dto.SaleId);
            if (sale == null)
                throw new NotFoundException("Sale with Id: " + dto.SaleId);

            bool productInDb = _dbContext.Products.Where(p => p.Id == dto.ProductId).Any();
            if (productInDb == false)
                throw new NotFoundException("Product with Id: " + dto.ProductId);

            // Create record
            ProductSold ps = _mapper.Map<ProductSold>(dto);
            try
            {
                _dbContext.ProductsSold.Add(ps);
                _dbContext.SaveChanges();

                sale.SumValue = _dbContext.ProductsSold.Sum(ps => ps.PriceAtTheTimeOfSale * ps.AmountSold);
                _dbContext.SaveChanges();
            }
            catch (Exception exception)
            {
                throw new DatabaseOperationFailedException(exception.Message);
            }

            GetProductSoldDto outputDto = _mapper.Map<GetProductSoldDto>(ps);

            return outputDto;
        }

        public int Delete(int id)
        {
            // Check if record exists
            ProductSold? ps = _dbContext.ProductsSold.FirstOrDefault(ps => ps.Id == id);
            if (ps == null)
                throw new NotFoundException("ProductSold with Id: " + id);

            // Delete record
            try
            {
                _dbContext.ProductsSold.Remove(ps);
                _dbContext.SaveChanges();
            }
            catch (Exception exception)
            {
                throw new DatabaseOperationFailedException(exception.Message);
            }

            return 0;
        }

        public GetProductSoldDto Read(int id)
        {
            // Check if record exists
            ProductSold? ps = _dbContext.ProductsSold.FirstOrDefault(ps => ps.Id == id);
            if (ps == null)
                throw new NotFoundException("ProductSold with Id: " + id);

            // Return dto
            GetProductSoldDto outputDto = _mapper.Map<GetProductSoldDto>(ps);

            return outputDto;
        }

        public GetProductSoldDto[] ReadAll()
        {
            ProductSold[] productsSold = _dbContext.ProductsSold.ToArray();
            GetProductSoldDto[] outputDto = _mapper.Map<GetProductSoldDto[]>(productsSold);

            return outputDto;
        }

        public GetProductSoldDto Update(UpdateProductSoldDto dto)
        {
            // Check dto
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (dto.Id == null)
                throw new BadRequestException("Dto is missing Id field");

            ProductSold? ps = _dbContext.ProductsSold.FirstOrDefault(ps => ps.Id == dto.Id);
            if (ps == null)
                throw new NotFoundException("ProductSold with Id: " + dto.Id);

            Product? product = _dbContext.Products.FirstOrDefault(p => p.Id == dto.ProductId);
            if (product == null)
                throw new NotFoundException("Product with Id: " + dto.Id);

            Sale? sale = _dbContext.Sales.FirstOrDefault(s => s.Id == dto.SaleId);
            if (sale == null)
                throw new NotFoundException("Sale with Id: " + dto.Id);

            ProductSold mappedFromDto = _mapper.Map<ProductSold>(dto);
            ps = Tools.UpdateObjectProperties(ps, mappedFromDto);

            // Save changes
            try
            {
                _dbContext.SaveChanges();

                sale.SumValue = _dbContext.ProductsSold.Sum(ps => ps.PriceAtTheTimeOfSale * ps.AmountSold);
                _dbContext.SaveChanges();
            }
            catch (Exception exception)
            {
                throw new DatabaseOperationFailedException(exception.Message);
            }

            GetProductSoldDto outputDto = _mapper.Map<GetProductSoldDto>(dto);

            return outputDto;
        }
    }
}
