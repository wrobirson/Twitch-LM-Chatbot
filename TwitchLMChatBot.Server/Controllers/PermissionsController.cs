using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwitchLMChatBot.Application.Abstractions;
using TwitchLMChatBot.Application.Services;

namespace TwitchLMChatBot.Server.Controllers
{
    [Route("api/permissions")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly IAccessControlService _accessControlService;

        public PermissionsController(IAccessControlService accessControlService)
        {
            _accessControlService = accessControlService;
        }

        [HttpGet(Name =nameof(GetPermissions))]
        public IActionResult GetPermissions()
        {
            return Ok(_accessControlService.Get());
        }

        [HttpPut(Name = nameof(SetPermissions))]
        public void SetPermissions(SetAccessControlRequest request)
        {
            _accessControlService.Set(request);
        }
    }
}
