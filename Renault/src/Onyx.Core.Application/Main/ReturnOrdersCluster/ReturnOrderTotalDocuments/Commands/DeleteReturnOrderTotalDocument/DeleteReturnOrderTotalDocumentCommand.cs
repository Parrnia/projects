using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Services;
using Onyx.Domain.Entities.ReturnOrdersCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderTotalDocuments.Commands.DeleteReturnOrderTotalDocument;

public record DeleteReturnOrderTotalDocumentCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public int ReturnOrderId { get; init; }
    public int ReturnOrderTotalId { get; init; }
};

public class DeleteReturnOrderTotalDocumentCommandHandler : IRequestHandler<DeleteReturnOrderTotalDocumentCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public DeleteReturnOrderTotalDocumentCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<Unit> Handle(DeleteReturnOrderTotalDocumentCommand request, CancellationToken cancellationToken)
    {
        var returnOrder = await _context.ReturnOrders
            .Include(c => c.ReturnOrderStateHistory)
            .Include(c => c.Totals)
            .ThenInclude(c => c.ReturnOrderTotalDocuments)
            .SingleOrDefaultAsync(e => e.Id == request.ReturnOrderId, cancellationToken);

        if (returnOrder == null)
        {
            throw new NotFoundException(nameof(ReturnOrder), request.ReturnOrderId);
        }
        if (returnOrder.GetCurrentReturnOrderState().ReturnOrderStatus != ReturnOrderStatus.Received)
        {
            throw new BadCommandException("به دلیل عدم همخوانی وضعیت سفارش بازگشت");
        }

        var returnOrderTotal = returnOrder.Totals.SingleOrDefault(c => c.Id == request.ReturnOrderTotalId);

        if (returnOrderTotal == null)
        {
            throw new NotFoundException(nameof(ReturnOrderTotal), request.ReturnOrderTotalId);
        }

        var returnOrderTotalDocument = returnOrderTotal.ReturnOrderTotalDocuments.SingleOrDefault(c => c.Id == request.Id);

        if (returnOrderTotalDocument == null)
        {
            throw new NotFoundException(nameof(ReturnOrderTotalDocument), request.Id);
        }

        await _fileStore.RemoveStoredFile(returnOrderTotalDocument.Image, cancellationToken);
        returnOrder.RemoveReturnOrderTotalDocumentWithoutValidation(returnOrderTotalDocument, request.ReturnOrderTotalId);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}