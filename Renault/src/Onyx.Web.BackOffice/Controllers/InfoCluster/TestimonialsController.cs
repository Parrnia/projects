using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.InfoCluster.Testimonials.Commands.CreateTestimonial;
using Onyx.Application.Main.InfoCluster.Testimonials.Commands.DeleteTestimonial;
using Onyx.Application.Main.InfoCluster.Testimonials.Commands.UpdateTestimonial;
using Onyx.Application.Main.InfoCluster.Testimonials.Queries.BackOffice;
using Onyx.Application.Main.InfoCluster.Testimonials.Queries.BackOffice.GetTestimonial;
using Onyx.Application.Main.InfoCluster.Testimonials.Queries.BackOffice.GetTestimonials;
using Onyx.Application.Main.InfoCluster.Testimonials.Queries.BackOffice.GetTestimonialsWithPagination;
using Onyx.Application.Main.InfoCluster.Testimonials.Validators;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;
using OnyxAuth.Shared;
using Onyx.Application.Main.InfoCluster.Testimonials.Queries.Export.ExportExcelTestimonials;

namespace Onyx.Web.BackOffice.Controllers.InfoCluster;


public class TestimonialsController : ApiControllerBase
{
    [HttpGet]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<PaginatedList<TestimonialDto>>> GetTestimonialsWithPagination([FromQuery] GetTestimonialsWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("all")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<TestimonialDto>>> GetAllTestimonials()
    {
        return await Mediator.Send(new GetAllTestimonialsQuery());
    }

    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<TestimonialDto?>> GetTestimonialById(int id)
    {
        return await Mediator.Send(new GetTestimonialByIdQuery(id));
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelTestimonialsQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "Testimonials.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateTestimonialCommand command)
    {
        return await Mediator.Send(command);
    }
    [HttpPut("{urlId}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int urlId, UpdateTestimonialCommand command)
    {
        if (urlId != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }
    [HttpDelete("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteTestimonialCommand(id));

        return NoContent();
    }

    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRangeTestimonial([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeTestimonialCommand { Ids = ids };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }
    //Validators
    [HttpGet("isUniqueName")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueTestimonialName([FromQuery] UniqueTestimonialNameValidator query)
    {
        return await Mediator.Send(query);
    }
}
