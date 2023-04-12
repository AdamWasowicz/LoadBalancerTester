﻿using AutoMapper;
using LBT_Api.Entities;
using LBT_Api.Models.AddressDto;
using LBT_Api.Models.CompanyDto;
using LBT_Api.Models.ContactInfoDto;
using LBT_Api.Models.ProductDto;
using LBT_Api.Models.ProductSoldDto;
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
            CreateMap<Company, GetCompanyDto>();
            CreateMap<CreateCompanyDto, Company>();
            CreateMap<UpdateCompanyNameDto, Company>();
            CreateMap<Company, GetCompanyWithDependenciesDto>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.ContactInfo, opt => opt.MapFrom(src => src.ContactInfo));

            // ContactInfo
            CreateMap<ContactInfo, GetContactInfoDto>();
            CreateMap<CreateContactInfoDto, ContactInfo>();
            CreateMap<UpdateContactInfoDto, ContactInfo>();

            // Product
            CreateMap<Product, GetProductDto>();
            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();

            // ProductSold
            CreateMap<ProductSold, GetProductSoldDto>();
            CreateMap<CreateProductSoldDto, ProductSold>();
            CreateMap<CreateProductSold_IntegratedDto, ProductSold>();
            CreateMap<UpdateProductSoldDto, ProductSold>();
            CreateMap<UpdateProductSoldDto, GetProductSoldDto>();

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
