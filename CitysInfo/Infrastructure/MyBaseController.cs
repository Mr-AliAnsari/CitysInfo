using Microsoft.AspNetCore.Mvc;

namespace CitysInfo.Infrastructure
{
    [ApiController]
    //[ApiVersion("1")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class MyBaseController : ControllerBase
    {

    }
}
