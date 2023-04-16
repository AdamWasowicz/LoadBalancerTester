using AutoMapper;
using LBT_Api.Entities;
using LBT_Api.Exceptions;
using LBT_Api.Interfaces.Services;
using LBT_Api.Models.ProductDto;
using LBT_Api.Models.ProductSoldDto;
using LBT_Api.Other;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LBT_Api.Services
{
    public class ProductSoldService : IProductSoldService
    {
        private readonly LBT_DbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        private readonly ISaleService _saleService;

        public ProductSoldService(
            LBT_DbContext dbContext, 
            IMapper mapper, 
            IProductService productService,
            ISaleService saleService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _productService = productService;
            _saleService = saleService;
        }

        public GetProductSoldDto Create(CreateProductSoldDto dto)
        {
            // Check dto
            if (Tools.ModelIsValid(dto) == false)
                throw new InvalidModelException();

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

        public GetProductSoldWithDependenciesDto CreateWithDependencies(CreateProductSoldWithDependenciesDto dto)
        {
            // Check dto
            if (Tools.ModelIsValid(dto) == false)
                throw new InvalidModelException("Model is invalid");

            var transaction = _dbContext.Database.BeginTransaction();
            ProductSold ps = null;

            try
            {
                // Dependencies
                var product = _productService.CreateWithDependencies(dto.Product);
                var sale = _saleService.CreateWithDependencies(dto.Sale);

                // Main
                ps = new ProductSold
                {
                    ProductId = product.Id,
                    SaleId = sale.Id,
                    AmountSold = dto.AmountSold,
                    PriceAtTheTimeOfSale = product.PriceNow
                };

                // Save changes
                _dbContext.SaveChanges();
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                throw new DatabaseOperationFailedException(exception.Message);
            }

            // Return dto
            GetProductSoldWithDependenciesDto outputDto = _mapper.Map<GetProductSoldWithDependenciesDto>(ps);

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

        public GetProductSoldWithDependenciesDto ReadWithDependencies(int id)
        {
            ProductSold? ps = _dbContext.ProductsSold.FirstOrDefault(ps => ps.Id == id);
            if (ps == null)
                throw new NotFoundException("ProductSold with Id: " + id);

            GetProductSoldWithDependenciesDto outputDto = _mapper.Map<GetProductSoldWithDependenciesDto>(ps);

            return outputDto;
        }

        public GetProductSoldDto[] ReadAll()
        {
            ProductSold[] productsSold = _dbContext.ProductsSold.ToArray();
            GetProductSoldDto[] outputDto = _mapper.Map<GetProductSoldDto[]>(productsSold);

            return outputDto;
        }

        public GetProductSoldWithDependenciesDto[] ReadAllWithDependencies()
        {
            ProductSold[] pss = _dbContext.ProductsSold.ToArray();
            GetProductSoldWithDependenciesDto[] outputDto = _mapper.Map<GetProductSoldWithDependenciesDto[]>(pss);

            return outputDto;
        }

        public GetProductSoldDto Update(UpdateProductSoldPrice dto)
        {
            // Check dto
            if (Tools.ModelIsValid(dto) == false)
                throw new InvalidModelException("Model is invalid");

            ProductSold? ps = _dbContext.ProductsSold.FirstOrDefault(ps => ps.Id == dto.Id);
            if (ps == null)
                throw new NotFoundException("ProductSold with Id: " + dto.Id);

            Sale? sale = _dbContext.Sales.First(sale => sale.Id == ps.SaleId);
            if (sale == null)
                throw new NotFoundException("Sale with Id: " + ps.SaleId);

            ProductSold mappedFromDto = _mapper.Map<ProductSold>(dto);
            ps.AmountSold = dto.AmountSold == null 
                ? ps.AmountSold 
                : (int)dto.AmountSold;
            ps.PriceAtTheTimeOfSale = dto.PriceAtTheTimeOfSale == null 
                ? ps.PriceAtTheTimeOfSale 
                : (int)dto.PriceAtTheTimeOfSale;

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
