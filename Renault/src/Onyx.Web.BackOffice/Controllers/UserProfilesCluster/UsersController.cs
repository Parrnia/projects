using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.UserProfilesCluster.Users.Commands.CreateUser;
using Onyx.Application.Main.UserProfilesCluster.Users.Commands.DeleteUser;
using Onyx.Application.Main.UserProfilesCluster.Users.Commands.UpdateUser;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;
using OnyxAuth.Shared;
using Onyx.Application.Main.UserProfilesCluster.Users.Queries.BackOffice;
using Onyx.Application.Main.UserProfilesCluster.Users.Queries.BackOffice.GetUser;
using Onyx.Application.Main.UserProfilesCluster.Users.Queries.BackOffice.GetUsers;

namespace Onyx.Web.BackOffice.Controllers.UserProfilesCluster;


public class UsersController : ApiControllerBase
{
    
    [HttpGet("all")]
    [CheckPermission(Roles.Employee, UserPermissions.UserManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<UserDto>>> GetAllUsers()
    {
        return await Mediator.Send(new GetAllUsersQuery());
    }
    
    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.UserManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<UserDto?>> GetUserById(Guid id)
    {
        return await Mediator.Send(new GetUserByIdQuery(id));
    }
    
    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.UserManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<Guid>> Create(CreateUserCommand command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpPut("{urlId}")]
    [CheckPermission(Roles.Employee, UserPermissions.UserManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(Guid urlId, UpdateUserCommand command)
    {
        if (urlId != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.UserManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Delete(Guid id)
    {
        await Mediator.Send(new DeleteUserCommand(id));

        return NoContent();
    }
}
