using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Settings;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.FrontOffice.GetInvoice;

public record GetReturnOrderInvoiceQuery(int Id, Guid? CustomerId) : IRequest<ReturnOrderInvoiceDto?>;

public class GetReturnOrderInvoiceQueryHandler : IRequestHandler<GetReturnOrderInvoiceQuery, ReturnOrderInvoiceDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ApplicationSettings _applicationSettings;
    private readonly ISevenSoftService _sevenSoftService;

    public GetReturnOrderInvoiceQueryHandler(IApplicationDbContext context, IMapper mapper, IOptions<ApplicationSettings> applicationSettings, ISevenSoftService sevenSoftService)
    {
        _context = context;
        _mapper = mapper;
        _sevenSoftService = sevenSoftService;
        _applicationSettings = applicationSettings.Value;
    }

    public async Task<ReturnOrderInvoiceDto?> Handle(GetReturnOrderInvoiceQuery request, CancellationToken cancellationToken)
    {
        var returnOrderInvoice = await _context.ReturnOrders
            .ProjectTo<ReturnOrderInvoiceDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (returnOrderInvoice == null)
        {
            return null;
        }
        if (request.CustomerId != null && returnOrderInvoice.CustomerId != request.CustomerId)
        {
            throw new ForbiddenAccessException("GetReturnOrderInvoiceQuery");
        }

        //getReturnOrderInvoiceFromSeven
        //var sevenReturnOrderInvoice = await _sevenSoftService.GetReturnOrderInvoice(ReturnOrderInvoice.Token, cancellationToken);
        //if (sevenReturnOrderInvoice != null)
        //{
        //    ReturnOrderInvoice.SaleNumber = sevenReturnOrderInvoice.SpExchanges.DealerSystemCode.ToString();
        //    ReturnOrderInvoice.ReturnOrderInvoiceSerial = sevenReturnOrderInvoice.SpExchanges.BranchCollectiveReturnOrderInvoiceNumberByYear.ToString();
        //    ReturnOrderInvoice.ReturnOrderInvoiceNumber = sevenReturnOrderInvoice.SpExchanges.BranchFactorCode.ToString();
        //    ReturnOrderInvoice.ReturnOrderInvoiceDate = sevenReturnOrderInvoice.SpExchanges.StrFactorDate;
        //    ReturnOrderInvoice.ReturnOrderInvoiceTime = sevenReturnOrderInvoice.SpExchanges.StrFactorTime;
        //    ReturnOrderInvoice.PreReturnOrderInvoiceNumber = sevenReturnOrderInvoice.SpExchanges.BranchPreFactorCode.ToString();
        //}
        //else
        //{
        //    throw new SevenException("ارتباط با سرور دچار مشکل شده است، لطفا مدتی بعد دوباره تلاش کنید");
        //}


        return returnOrderInvoice;
    }
}
