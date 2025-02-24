using DocumentFormat.OpenXml.Vml.Office;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Services;
using Onyx.Domain.Entities.ReturnOrdersCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItemDocuments.Commands.UpdateReturnOrderItemDocument;
public record UpdateReturnOrderItemDocumentCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public Guid Image { get; init; }
    public string Description { get; init; } = null!;
    public int ReturnOrderItemId { get; init; }
    public int ReturnOrderItemGroupId { get; init; }
    public int ReturnOrderId { get; init; }
}

public class UpdateReturnOrderItemDocumentCommandHandler : IRequestHandler<UpdateReturnOrderItemDocumentCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public UpdateReturnOrderItemDocumentCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<Unit> Handle(UpdateReturnOrderItemDocumentCommand request, CancellationToken cancellationToken)
    {
        var returnOrder = await _context.ReturnOrders
            .Include(c => c.ReturnOrderStateHistory)
            .Include(c => c.ItemGroups)
            .ThenInclude(c => c.ReturnOrderItems)
            .Include(c => c.ItemGroups)
            .ThenInclude(c => c.ReturnOrderItems)
            .ThenInclude(c => c.ReturnOrderItemDocuments)
            .Include(c => c.Order)
            .ThenInclude(c => c.Items)
            .SingleOrDefaultAsync(e => e.Id == request.ReturnOrderId, cancellationToken);

        if (returnOrder == null)
        {
            throw new NotFoundException(nameof(ReturnOrder), request.ReturnOrderId);
        }
        if (returnOrder.GetCurrentReturnOrderState().ReturnOrderStatus != ReturnOrderStatus.Received)
        {
            throw new BadCommandException("به دلیل عدم همخوانی وضعیت سفارش بازگشت");
        }
        var returnOrderItemGroup = returnOrder.ItemGroups.SingleOrDefault(c => c.Id == request.ReturnOrderItemGroupId);

        if (returnOrderItemGroup == null)
        {
            throw new NotFoundException(nameof(ReturnOrderItemGroup), request.ReturnOrderItemGroupId);
        }

        var returnOrderItem = returnOrderItemGroup.ReturnOrderItems.SingleOrDefault(c => c.Id == request.ReturnOrderItemId);

        if (returnOrderItem == null)
        {
            throw new NotFoundException(nameof(ReturnOrderItem), request.ReturnOrderItemId);
        }

        var returnOrderItemDocument = returnOrderItem.ReturnOrderItemDocuments.SingleOrDefault(c => c.Id == request.Id);

        if (returnOrderItemDocument == null)
        {
            throw new NotFoundException(nameof(ReturnOrderItemDocument), request.Id);
        }
        if (returnOrderItemDocument.Image != request.Image)
        {
            await _fileStore.RemoveStoredFile(returnOrderItemDocument.Image, cancellationToken);
            await _fileStore.SaveStoredFile(
                    request.Image,
                    FileCategory.ReturnOrderDocument.ToString(),
                    FileCategory.ReturnOrderDocument,
                    null,
                false,
                    cancellationToken);
        }

        returnOrder.UpdateReturnOrderItemDocumentWithoutValidation(request.Id, request.Image, request.Description, request.ReturnOrderItemGroupId, request.ReturnOrderItemId);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
