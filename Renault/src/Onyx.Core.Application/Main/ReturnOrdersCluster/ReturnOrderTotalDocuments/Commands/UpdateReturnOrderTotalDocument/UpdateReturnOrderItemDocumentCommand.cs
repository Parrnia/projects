using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ReturnOrdersCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderTotalDocuments.Commands.UpdateReturnOrderTotalDocument;
public record UpdateReturnOrderTotalDocumentCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public Guid Image { get; init; }
    public string Description { get; init; } = null!;
    public int ReturnOrderTotalId { get; init; }
    public int ReturnOrderId { get; init; }
}

public class UpdateReturnOrderTotalDocumentCommandHandler : IRequestHandler<UpdateReturnOrderTotalDocumentCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public UpdateReturnOrderTotalDocumentCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<Unit> Handle(UpdateReturnOrderTotalDocumentCommand request, CancellationToken cancellationToken)
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

        if (returnOrderTotalDocument.Image != request.Image)
        {
            await _fileStore.RemoveStoredFile(returnOrderTotalDocument.Image, cancellationToken);
            await _fileStore.SaveStoredFile(
                request.Image,
                FileCategory.ReturnOrderDocument.ToString(),
                FileCategory.ReturnOrderDocument,
                null,
                false,
                cancellationToken);
        }

        returnOrder.UpdateReturnOrderTotalDocumentWithoutValidation(request.Id, request.Image, request.Description, request.ReturnOrderTotalId);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
