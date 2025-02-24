using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.Providers.Commands.UpdateProvider;
public record UpdateProviderCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public int Code { get; init; }
    public string LocalizedName { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string? LocalizedCode { get; init; }
    public string? Description { get; init; }
}

public class UpdateProviderCommandHandler : IRequestHandler<UpdateProviderCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateProviderCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateProviderCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Providers
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Provider), request.Id);
        }


        entity.Code = request.Code;
        entity.LocalizedName = request.LocalizedName;
        entity.Name = request.Name;
        entity.LocalizedCode = request.LocalizedCode;
        entity.Description = request.Description;


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}