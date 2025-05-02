using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Profile for mapping between Sale entity and UpdateSaleResponse
/// </summary>
public class UpdateSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for UpdateSale operation
    /// </summary>
    public UpdateSaleProfile()
    {
        CreateMap<UpdateSaleCommand, Sale>()
            .ForMember(dest => dest.SaleNumber, opt => opt.Ignore()) // Example: Ignore unmapped properties
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => SaleStatus.Active)) // Set default values
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items.ToList())); // Map nested collections

        CreateMap<Sale, UpdateSaleResult>()
            .ForMember(dest => dest.CustomerId, opt => opt.Ignore()) // Adjust as needed
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalValue));
    }
}