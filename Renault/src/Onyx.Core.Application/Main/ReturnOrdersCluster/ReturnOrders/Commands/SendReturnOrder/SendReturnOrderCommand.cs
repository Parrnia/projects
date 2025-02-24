using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.ReturnOrderModels;
using Onyx.Domain.Entities.ReturnOrdersCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.SendReturnOrder;
public record SendReturnOrderCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string Details { get; init; } = null!;
    public ReturnOrderTransportationType ReturnOrderTransportationType { get; init; }
    public decimal? ReturnShippingPrice { get; init; }
    public List<DocumentCommandForTotal> DocumentCommandForTotals { get; init; } = new List<DocumentCommandForTotal>();
}

public class SendReturnOrderCommandHandler : IRequestHandler<SendReturnOrderCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public SendReturnOrderCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<Unit> Handle(SendReturnOrderCommand request, CancellationToken cancellationToken)
    {
        var returnOrder = await _context.ReturnOrders
            .Include(c => c.Order)
            .Include(c => c.Totals)
            .ThenInclude(c => c.ReturnOrderTotalDocuments)
            .Include(c => c.ReturnOrderStateHistory)
            .SingleOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (returnOrder == null)
        {
            throw new NotFoundException(nameof(ReturnOrder), request.Id);
        }

        if (request.ReturnShippingPrice != null)
        {
            var orderTotal = new ReturnOrderTotal()
            {
                Title = "بازگشت",
                Price = (decimal)request.ReturnShippingPrice,
                Type = ReturnOrderTotalType.ReturnShipping,
                ReturnOrderTotalApplyType = ReturnOrderTotalApplyType.DoNothing
            };
            foreach (var documentCommandForTotal in request.DocumentCommandForTotals)
            {
                var item = new ReturnOrderTotalDocument();
                await _fileStore.SaveStoredFile(
                    documentCommandForTotal.Image,
                    FileCategory.ReturnOrderDocument.ToString(),
                    FileCategory.ReturnOrderDocument,
                    null,
                    false,
                    cancellationToken);
                item.Image = documentCommandForTotal.Image;
                item.Description = documentCommandForTotal.Description;
                orderTotal.ReturnOrderTotalDocuments.Add(item);
            }
            returnOrder.AddTotalWithoutValidation(orderTotal);
            returnOrder.CheckReturnOrderTotals();
        }
        returnOrder.ReturnOrderTransportationType = request.ReturnOrderTransportationType;


        returnOrder.Send(request.Details);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
