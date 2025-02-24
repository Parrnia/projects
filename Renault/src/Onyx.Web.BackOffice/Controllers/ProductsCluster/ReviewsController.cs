using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.ProductsCluster.Reviews.Commands.DeleteReview;
using Onyx.Application.Main.ProductsCluster.Reviews.Commands.UpdateReview;
using Onyx.Application.Main.ProductsCluster.Reviews.Queries.BackOffice;
using Onyx.Application.Main.ProductsCluster.Reviews.Queries.BackOffice.GetReview;
using Onyx.Application.Main.ProductsCluster.Reviews.Queries.BackOffice.GetReviews;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;
using OnyxAuth.Shared;
using Onyx.Application.Main.ProductsCluster.Reviews.Queries.BackOffice.GetReviewsWithPagination;
using Onyx.Application.Main.ProductsCluster.Reviews.Queries.Export.ExportExcelReviews;

namespace Onyx.Web.BackOffice.Controllers.ProductsCluster;


public class ReviewsController : ApiControllerBase
{
    [HttpGet("all")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ReviewDto>>> GetAllReviews()
    {
        return await Mediator.Send(new GetAllReviewsQuery());
    }

    [HttpGet]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<FilteredReviewDto>> GetReviewsWithPagination([FromQuery] GetReviewsWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("customer{customerId}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ReviewDto>>> GetReviewsByCustomerId(Guid customerId)
    {
        return await Mediator.Send(new GetReviewsByCustomerIdQuery(customerId));
    }

    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<ReviewDto?>> GetReviewById(int id)
    {
        return await Mediator.Send(new GetReviewByIdQuery(id));
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelReviewsQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "Reviews.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }



    [HttpPut("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int id, UpdateReviewCommand command)
    {
        if (id != command.Id)
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
        await Mediator.Send(new DeleteReviewCommand(id,null));

        return NoContent();
    }

    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRangeReview([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeReviewCommand { Ids = ids };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }
}
