using AutoMapper;
using LBT_Api.Entities;
using LBT_Api.Models.AddressDto;
using LBT_Api.Models.CompanyDto;
using LBT_Api.Models.ContactInfoDto;
using LBT_Api.Models.ProductDto;
using LBT_Api.Models.SaleDto;
using LBT_Api.Models.SupplierDto;
using LBT_Api.Models.WorkerDto;

namespace LBT_Api.Other
{
    public class LBT_Entity_MappingProfile : Profile
    {
        public LBT_Entity_MappingProfile()
        {
            // Address
            CreateMap<Address, GetAddressDto>();
            CreateMap<CreateAddressDto, Address>();
            CreateMap<UpdateAddressDto, Address>();

            // Company
            CreateMap<Company, GetAddressDto>();
            CreateMap<CreateCompanyDto, Company>();
            CreateMap<UpdateCompanyDto, Company>();

            // ContactInfo
            CreateMap<ContactInfo, GetContactInfoDto>();
            CreateMap<CreateContactInfoDto, ContactInfo>();
            CreateMap<UpdateContactInfoDto, ContactInfo>();

            // Product
            CreateMap<Product, GetProductDto>();
            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();

            // Sale
            CreateMap<Sale, GetSaleDto>();
            CreateMap<CreateSaleDto, Sale>();
            CreateMap<UpdateSaleDto, Sale>();

            // Supplier
            CreateMap<Supplier, GetSupplierDto>();
            CreateMap<CreateSupplierDto, Supplier>();
            CreateMap<UpdateSupplierDto, Supplier>();

            // Worker
            CreateMap<Worker, GetWorkerDto>();
            CreateMap<CreateWorkerDto, Worker>();
            CreateMap<UpdateWorkerDto, Worker>();
        }
    }
}
