using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.ProductsCluster.Reviews.Commands.CreateReview;
using Onyx.Application.Main.ProductsCluster.Reviews.Commands.DeleteReview;
using Onyx.Application.Main.ProductsCluster.Reviews.Queries.FrontOffice.GetReview;
using Onyx.Application.Main.ProductsCluster.Reviews.Queries.FrontOffice.GetReviews.GetReviewsByCustomerId;
using Onyx.Application.Main.ProductsCluster.Reviews.Queries.FrontOffice.GetReviews.GetReviewsByProductId;
using Onyx.Application.Main.ProductsCluster.Reviews.Queries.FrontOffice.GetReviewsWithPagination;
using Onyx.Web.FrontOffice.Authorization;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared;

namespace Onyx.Web.FrontOffice.Controllers.ProductsCluster;


public class ReviewsController : ApiControllerBase
{
    [HttpGet("product{productId}")]
    public async Task<ActionResult<List<ReviewByProductIdDto>>> GetReviewsByProductId(int productId)
    {
        return await Mediator.Send(new GetReviewsByProductIdQuery(productId));
    }

    [HttpGet("byProductIdWithPagination")]
    public async Task<ActionResult<PaginatedList<ReviewByProductIdWithPaginationDto>>> GetReviewsByProductIdWithPagination([FromQuery] GetReviewsByProductIdWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ReviewByIdDto?>> GetReviewById(int id)
    {
        return await Mediator.Send(new GetReviewByIdQuery(id));
    }
    [HttpGet("selfCustomer")]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult<List<ReviewByCustomerIdDto>>> GetReviewsByCustomerId()
    {
        if (UserInfo == null)
        {
            return BadRequest();
        }
        return await Mediator.Send(new GetReviewsByCustomerIdQuery(UserInfo.UserId));
    }
    [HttpPost]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult<int>> Create(CreateReviewCommand command)
    {
        if (UserInfo == null)
        {
            return BadRequest();
        }

        command.CustomerId = UserInfo.UserId;
        command.AuthorName = UserInfo.FirstName + " " + UserInfo.LastName;

        return await Mediator.Send(command);
    }
    
    [HttpDelete("self{id}")]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult> SelfDelete(int id)
    {
        if (UserInfo == null)
        {
            return BadRequest();
        }
        await Mediator.Send(new DeleteReviewCommand(id, UserInfo.UserId));

        return NoContent();
    }
}
