using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwitchLMChatBot.Application.Abstractions;
using TwitchLMChatBot.Models;

namespace TwitchLMChatBot.Server.Controllers
{
    [Route("api/commands")]
    [ApiController]
    public class CommandController : ControllerBase
    {
        private readonly ICommandRespository _commandRespository;

        public CommandController(ICommandRespository commandRespository)
        {
            _commandRespository = commandRespository;
        }

        [HttpGet(Name =nameof(ListCommands))]
        [ProducesResponseType<Command[]>(StatusCodes.Status200OK)]
        public IActionResult ListCommands() {
            return Ok(_commandRespository.FindAll()); 
        }

        [HttpGet("{id}", Name = nameof(FindCommand))]
        [ProducesResponseType<Command>(StatusCodes.Status200OK)]
        public IActionResult FindCommand(int id)
        {
            var command = _commandRespository.FindById(id);
            return Ok(command);
        }

        public class CreateCommandRequest
        {
            public string Name { get; set; }

            public bool UsingAI { get; set; }

            public string Response { get; set; }

            public CommandPermissions Permissions { get; set; }
        }

        [HttpPost(Name =nameof(CreateCommand))]
        [ProducesResponseType<Command>(StatusCodes.Status201Created)]
        public IActionResult CreateCommand(CreateCommandRequest request)
        {
            var command = new Command();
            command.Name = request.Name;
            command.Response = request.Response;
            command.IsEnabled = true;
            command.Permissions = request.Permissions;
            _commandRespository.Insert(command);
            return Created(Url.Action(
                nameof(FindCommand), new { command.Id }), 
                command);
        }


        public class UpdateCommandRequest
        {
            public string Name { get; set; }

            public bool UsingAI { get; set; }

            public string Response { get; set; }

            public CommandPermissions Permissions { get; set; }

        }


        [HttpPut("{id}",Name =nameof(UpdateCommand))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdateCommand(int id, UpdateCommandRequest request)
        {
            var command = _commandRespository.FindById(id);
            command.Name = request.Name;
            command.UsingAI = request.UsingAI;
            command.Response = request.Response;
            command.Permissions = request.Permissions;
            _commandRespository.Update(command);
            return NoContent();
        }


        [HttpDelete("{id}", Name =nameof(DeleteCommand))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult DeleteCommand(int id)
        {
            var command = _commandRespository.FindById(id);
            _commandRespository.Delete(command);
            return NoContent();
        }
    }
}
