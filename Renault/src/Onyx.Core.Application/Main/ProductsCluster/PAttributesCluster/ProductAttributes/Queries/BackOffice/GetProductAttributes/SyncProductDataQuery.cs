using AutoMapper;
using MediatR;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributes.Queries.BackOffice.GetProductAttributes;

public record SyncProductDataQuery : IRequest<List<ProductAttributeDto>>;

public class SyncProductDataQueryHandler : IRequestHandler<SyncProductDataQuery, List<ProductAttributeDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public SyncProductDataQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProductAttributeDto>> Handle(SyncProductDataQuery request, CancellationToken cancellationToken)
    {
        //Bussiness Must be here
        var synData = new List<ProductAttributeDto>();
        return synData;
    }
}

