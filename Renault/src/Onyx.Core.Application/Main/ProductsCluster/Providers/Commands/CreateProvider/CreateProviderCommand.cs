using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.Providers.Commands.CreateProvider;
public record CreateProviderCommand : IRequest<int>
{
    public int Code { get; init; }
    public string LocalizedName { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string? LocalizedCode { get; init; }
    public string? Description { get; init; }
}

public class CreateProviderCommandHandler : IRequestHandler<CreateProviderCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateProviderCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateProviderCommand request, CancellationToken cancellationToken)
    {
        var entity = new Provider()
        {
            Code = request.Code,
            LocalizedName = request.LocalizedName,
            Name = request.Name,
            LocalizedCode = request.LocalizedCode,
            Description = request.Description
        };


        _context.Providers.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
