using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controller
{
    [ApiController]
    [Authorize]
    [Route("[controller]/[action]")]
    public class BaseApiController : ControllerBase
    {
    }
}
