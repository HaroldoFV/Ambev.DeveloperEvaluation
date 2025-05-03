using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelItem;

/// <summary>
/// Profile for mapping between SaleItem entity and CancelItemResult
/// </summary>
public class CancelItemProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CancelItem operation
    /// </summary>
    public CancelItemProfile()
    {
        CreateMap<(Sale sale, Guid itemId), CancelItemResult>()
            .ForMember(dest => dest.SaleId, opt => opt.MapFrom(src => src.sale.Id))
            .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.itemId))
            .ForMember(dest => dest.CancelledAt, opt =>
                opt.MapFrom(src => src.sale.Items.First(i => i.Id == src.itemId).CancelledAt!.Value));
    }
}