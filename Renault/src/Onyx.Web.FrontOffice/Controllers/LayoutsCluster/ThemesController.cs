using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.LayoutsCluster.Themes.Queries.FrontOffice.GetTheme;

namespace Onyx.Web.FrontOffice.Controllers.LayoutsCluster;

public class ThemesController : ApiControllerBase
{

    [HttpGet("default")]
    public async Task<ActionResult<ThemeDto?>> GetDefaultTheme()
    {
        return await Mediator.Send(new GetDefaultThemeQuery());
    }

}
