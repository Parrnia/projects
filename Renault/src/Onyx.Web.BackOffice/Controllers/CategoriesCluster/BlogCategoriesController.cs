using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.CategoriesCluster.BlogCategories.Commands.CreateBlogCategory;
using Onyx.Application.Main.CategoriesCluster.BlogCategories.Commands.DeleteBlogCategory;
using Onyx.Application.Main.CategoriesCluster.BlogCategories.Commands.UpdateBlogCategory;
using Onyx.Application.Main.CategoriesCluster.BlogCategories.Queries.BackOffice;
using Onyx.Application.Main.CategoriesCluster.BlogCategories.Queries.BackOffice.GetBlogCategories;
using Onyx.Application.Main.CategoriesCluster.BlogCategories.Queries.BackOffice.GetBlogCategory;
using Onyx.Application.Main.CategoriesCluster.BlogCategories.Validators;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;
using OnyxAuth.Shared;

namespace Onyx.Web.BackOffice.Controllers.CategoriesCluster;

public class BlogCategoriesController : ApiControllerBase
{
    [HttpGet("all")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<BlogCategoryDto>>> GetAllBlogCategories()
    {
        return await Mediator.Send(new GetAllBlogCategoriesQuery());
    }

    [HttpGet("blogParentCategory")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<BlogCategoryDto>>> GetBlogCategoriesByBlogParentCategoryId([FromQuery] GetBlogCategoriesByBlogParentCategoryIdQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<BlogCategoryDto?>> GetBlogCategoryById(int id)
    {
        return await Mediator.Send(new GetBlogCategoryByIdQuery(id));
    }

    [HttpGet("slug")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<BlogCategoryDto?>> GetBlogCategoryBySlug([FromQuery] GetBlogCategoryBySlugQuery query)
    {
        return await Mediator.Send(query);
    }
    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateBlogCategoryCommand command)
    {
        return await Mediator.Send(command);
    }
    [HttpPut("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int id, UpdateBlogCategoryCommand command)
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
        await Mediator.Send(new DeleteBlogCategoryCommand(id));

        return NoContent();
    }
    //Validators
    [HttpGet("isUniqueLocalizedName")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueBlogCategoryLocalizedName([FromQuery] UniqueBlogCategoryLocalizedNameValidator query)
    {
        return await Mediator.Send(query);
    }
    [HttpGet("isUniqueName")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueBlogCategoryName([FromQuery] UniqueBlogCategoryNameValidator query)
    {
        return await Mediator.Send(query);
    }
}
