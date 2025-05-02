using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

/// <summary>
/// Profile for mapping between Sale entity and CancelSaleResult
/// </summary>
public class CancelSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CancelSale operation
    /// </summary>
    public CancelSaleProfile()
    {
        CreateMap<CancelSaleCommand, Sale>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.SaleId));

        CreateMap<Sale, CancelSaleResult>()
            .ForMember(dest => dest.SaleId, opt => opt.MapFrom(src => src.Id));
    }
}