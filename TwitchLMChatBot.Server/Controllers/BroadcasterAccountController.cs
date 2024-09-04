using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwitchLMChatBot.Application.Abstractions;
using TwitchLMChatBot.Models;

namespace TwitchLMChatBot.Server.Controllers
{
    [Route("api/broadcaster-account")]
    [ApiController]
    public class BroadcasterAccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public BroadcasterAccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpGet]
        [ProducesResponseType<AccountUser>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        public IActionResult Get()
        {
            var account = _accountRepository.FindByType(AccountType.Broadcaster);
            if (account == null)
            {
                return NoContent();
            }

            return Ok(account.User);
        }

        [HttpDelete]
        [ProducesResponseType<AccountUser>(StatusCodes.Status204NoContent)]
        public IActionResult Delete()
        {
            var account = _accountRepository.FindByType(AccountType.Broadcaster);
            if (account == null)
            {
                return NotFound();
            }
            _accountRepository.Delete(account);
            return NoContent();
        }
    }
}
