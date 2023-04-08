using AutoMapper;
using LBT_Api.Entities;
using LBT_Api.Exceptions;
using LBT_Api.Interfaces.Services;
using LBT_Api.Models.AddressDto;
using LBT_Api.Models.SaleDto;
using LBT_Api.Other;

namespace LBT_Api.Services
{
    public class SaleService : ISaleService
    {
        private readonly LBT_DbContext _dbContext;
        private readonly IMapper _mapper;

        public SaleService(LBT_DbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public GetSaleDto Create(CreateSaleDto dto)
        {
            // Check dto
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            bool dtoIsValid = Tools.AllStringPropsAreNotNull(dto);
            if (dtoIsValid == false)
                throw new BadRequestException("Dto is missing fields");

            if (dto.WorkerId == null)
                throw new BadRequestException("Dto is missing fields");

            bool workerExists = _dbContext.Workers.Where(w => w.Id == dto.WorkerId).Any();
            if (workerExists == false)
                throw new NotFoundException("Worker with Id: " + dto.WorkerId);

            // Create records
            Sale sale = new Sale()
            {
                SaleDate = DateTime.Now,
                WorkerId = (int)dto.WorkerId,
                SumValue = 0,
            };

            var transaction = _dbContext.Database.BeginTransaction();
            try
            {

                _dbContext.Sales.Add(sale);
                _dbContext.SaveChanges();

                // Create sold products rows
                foreach (var psdto in dto.ProductsSold)
                {
                    double? priceAtTheTimeOfSale = 0;
                    try
                    {
                        priceAtTheTimeOfSale = _dbContext.Products.FirstOrDefault(p => p.Id == psdto.ProductId).PriceNow;
                    }
                    catch (Exception exception)
                    {
                        throw new DatabaseOperationFailedException(exception.Message);
                    }

                    if (priceAtTheTimeOfSale == null)
                        throw new NotFoundException("Product with Id: " + psdto.ProductId);

                    ProductSold productSold = new ProductSold()
                    {
                        AmountSold = psdto.AmountSold,
                        PriceAtTheTimeOfSale = (double)priceAtTheTimeOfSale,
                        ProductId = psdto.ProductId,
                        SaleId = sale.Id,
                    };

                    _dbContext.ProductsSold.Add(productSold);
                    _dbContext.SaveChanges();
                }

                ProductSold[] productSoldInSale = _dbContext.ProductsSold.Where(ps => ps.SaleId == sale.Id).ToArray();
                double sumValue = productSoldInSale.Sum(ps => ps.PriceAtTheTimeOfSale * ps.AmountSold);

                sale.SumValue = sumValue;
                _dbContext.SaveChanges();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }

            transaction.Commit();
            GetSaleDto outputDto = _mapper.Map<GetSaleDto>(sale);

            return outputDto;

        }

        public int Delete(int id)
        {
            // Check if record exists
            Sale? sale = _dbContext.Sales.FirstOrDefault(s => s.Id == id);
            if (sale == null)
                throw new NotFoundException("Sale with Id: " + id);

            // Delete records
            var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                _dbContext.Sales.Remove(sale);
                _dbContext.SaveChanges();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }

            transaction.Commit();

            return 0;
        }

        public GetSaleDto Read(int id)
        {
            // Check if record exists
            Sale? sale = _dbContext.Sales.FirstOrDefault(a => a.Id == id);
            if (sale == null)
                throw new NotFoundException("Sale with Id: " + id);

            // Return dto
            GetSaleDto outputDto = _mapper.Map<GetSaleDto>(sale);

            return outputDto;
        }

        public GetSaleDto[] ReadAll()
        {
            Sale[] sales = _dbContext.Sales.ToArray();
            GetSaleDto[] saleDtos = _mapper.Map<GetSaleDto[]>(sales);

            return saleDtos;
        }

        public GetSaleDto Update(UpdateSaleDto dto)
        {
            // Check dto
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (dto.Id == null)
                throw new BadRequestException("Dto is missing Id field");


            // Check if records exist
            Sale? sale = _dbContext.Sales.FirstOrDefault(s => s.Id == dto.Id);
            if (sale == null)
                throw new NotFoundException("Sale with Id: " + dto.Id);

            Worker? worker = _dbContext.Workers.FirstOrDefault(w => w.Id == dto.WorkerId);
            if (worker == null)
                throw new NotFoundException("Worker with Id: " + dto.Id);

            Sale mappedFromDto = _mapper.Map<Sale>(dto);
            sale = Tools.UpdateObjectProperties(sale, mappedFromDto);

            // Save changes
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception exception)
            {
                throw new DatabaseOperationFailedException(exception.Message);
            }

            GetSaleDto outputDto = _mapper.Map<GetSaleDto>(sale);

            return outputDto;
        }
    }
}
