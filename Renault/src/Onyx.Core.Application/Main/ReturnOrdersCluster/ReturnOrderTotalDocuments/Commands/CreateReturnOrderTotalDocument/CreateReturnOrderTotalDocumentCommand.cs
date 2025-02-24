using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ReturnOrdersCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderTotalDocuments.Commands.CreateReturnOrderTotalDocument;
public record CreateReturnOrderTotalDocumentCommand : IRequest<int>
{
    public Guid Image { get; init; }
    public string Description { get; init; } = null!;
    public int ReturnOrderTotalId { get; init; }
    public int ReturnOrderId { get; init; }
}

public class CreateReturnOrderTotalDocumentCommandHandler : IRequestHandler<CreateReturnOrderTotalDocumentCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public CreateReturnOrderTotalDocumentCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<int> Handle(CreateReturnOrderTotalDocumentCommand request, CancellationToken cancellationToken)
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

        await _fileStore.SaveStoredFile(
            request.Image,
            FileCategory.ReturnOrderDocument.ToString(),
            FileCategory.ReturnOrderDocument,
            null,
            false,
            cancellationToken);

        var entity = new ReturnOrderTotalDocument()
        {
            Image = request.Image,
            Description = request.Description,
        };

        returnOrder.AddReturnOrderTotalDocumentWithoutValidation(entity, request.ReturnOrderTotalId);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
