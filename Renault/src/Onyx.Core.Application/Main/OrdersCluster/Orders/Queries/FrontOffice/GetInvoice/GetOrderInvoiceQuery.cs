using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Settings;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.FrontOffice.GetInvoice;

public record GetOrderInvoiceQuery(int Id, Guid? CustomerId) : IRequest<OrderInvoiceDto?>;

public class GetOrderInvoiceQueryHandler : IRequestHandler<GetOrderInvoiceQuery, OrderInvoiceDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ApplicationSettings _applicationSettings;
    private readonly ISevenSoftService _sevenSoftService;

    public GetOrderInvoiceQueryHandler(IApplicationDbContext context, IMapper mapper, IOptions<ApplicationSettings> applicationSettings, ISevenSoftService sevenSoftService)
    {
        _context = context;
        _mapper = mapper;
        _sevenSoftService = sevenSoftService;
        _applicationSettings = applicationSettings.Value;
    }

    public async Task<OrderInvoiceDto?> Handle(GetOrderInvoiceQuery request, CancellationToken cancellationToken)
    {
        var orderInvoice = await _context.Orders
            .ProjectTo<OrderInvoiceDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (orderInvoice == null)
        {
            return null;
        }
        if (request.CustomerId != null && orderInvoice.CustomerId != request.CustomerId)
        {
            throw new ForbiddenAccessException("GetOrderInvoiceQuery");
        }

        //getOrderInvoiceFromSeven
        var sevenOrderInvoice = await _sevenSoftService.GetOrderInvoice(orderInvoice.Token, cancellationToken);
        if (sevenOrderInvoice != null)
        {
            orderInvoice.SaleNumber = sevenOrderInvoice.SpExchanges.DealerSystemCode.ToString();
            orderInvoice.OrderInvoiceSerial = sevenOrderInvoice.SpExchanges.BranchCollectiveInvoiceNumberByYear.ToString();
            orderInvoice.OrderInvoiceNumber = sevenOrderInvoice.SpExchanges.BranchFactorCode.ToString();
            orderInvoice.OrderInvoiceDate = sevenOrderInvoice.SpExchanges.StrFactorDate;
            orderInvoice.OrderInvoiceTime = sevenOrderInvoice.SpExchanges.StrFactorTime;
            orderInvoice.PreOrderInvoiceNumber = sevenOrderInvoice.SpExchanges.BranchPreFactorCode.ToString();
        }
        else
        {
            throw new SevenException("ارتباط با سرور دچار مشکل شده است، لطفا مدتی بعد دوباره تلاش کنید");
        }

        foreach (var orderInvoiceItemDto in orderInvoice.Items)
        {
            orderInvoiceItemDto.TaxPrice = (orderInvoiceItemDto.Price * (decimal) (1 - (orderInvoiceItemDto.TotalDiscountPercent/100))) * ((decimal)orderInvoice.TaxPercent/100);
        }


        return orderInvoice;
    }
}
