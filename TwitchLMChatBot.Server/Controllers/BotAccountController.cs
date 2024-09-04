using Microsoft.AspNetCore.Mvc;
using TwitchLMChatBot.Application.Abstractions;
using TwitchLMChatBot.Models;

namespace TwitchLMChatBot.Server.Controllers
{
    [Route("api/bot-account")]
    [ApiController]
    public class BotAccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public BotAccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpGet]
        [ProducesResponseType<AccountUser>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        public IActionResult Get()
        {
            var account = _accountRepository.FindByType(AccountType.Bot);
            if (account == null)
            {
                return NotFound();
            }

            return Ok(account.User);
        }

        [HttpDelete]
        [ProducesResponseType<AccountUser>(StatusCodes.Status204NoContent)]
        public IActionResult Delete()
        {
            var account = _accountRepository.FindByType(AccountType.Bot);
            if (account == null)
            {
                return NotFound();
            }
            _accountRepository.Delete(account);
            return NoContent();
        }
    }
}
