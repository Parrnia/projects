using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Application.Main.BrandsCluster.Models.Commands.UpdateModel;
public record UpdateModelCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string LocalizedName { get; init; } = null!;
    public string Name { get; init; } = null!;
    public int FamilyId { get; init; }
    public bool IsActive { get; init; }
}

public class UpdateModelCommandHandler : IRequestHandler<UpdateModelCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateModelCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateModelCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Models
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Model), request.Id);
        }

        entity.LocalizedName = request.LocalizedName;
        entity.Name = request.Name;
        entity.FamilyId = request.FamilyId;
        entity.IsActive = request.IsActive;

        //foreach (var kindCommandForModel in request.Kinds)
        //{
        //    if (kindCommandForModel.Id.HasValue)
        //    {
        //        var existingRecord = entity.Kinds?.FirstOrDefault(r => r.Id == kindCommandForModel.Id);
        //        if (existingRecord != null)
        //        {
        //            existingRecord.LocalizedName = kindCommandForModel.LocalizedName;
        //            existingRecord.Name = kindCommandForModel.Name;
        //        }
        //    }
        //    else
        //    {
        //        entity.Kinds?.Add(new Kind()
        //        {
        //            LocalizedName = kindCommandForModel.LocalizedName,
        //            Name = kindCommandForModel.Name
        //        });
        //    }
        //}

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
