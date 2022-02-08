using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PackIT.Infrastructure.Commands;
using PackIT.Infrastructure.Commands.Models;

namespace PackIT.Api.Controllers
{
    [Route("/api/PackingLists")]
    public class PackingListsCommandController : BaseController
    {
        private readonly IPackingListCommandService _handler;

        public PackingListsCommandController(IPackingListCommandService handler)
        {
            _handler = handler;
        }

        [HttpPut("items")]
        public async Task<IActionResult> Put([FromBody] AddPackingItem command)
        {
            await _handler.AddPackingItemAsync(command);
            return Ok();
        }

        [HttpPut("/items/pack")]
        public async Task<IActionResult> Put([FromBody] PackItem command)
        {
            await _handler.PackItemAsync(command);
            return Ok();
        }

        [HttpDelete("/items")]
        public async Task<IActionResult> Delete([FromBody] RemovePackingItem command)
        {
            await _handler.RemovePackingItemAsync(command);
            return Ok();
        }

        [HttpDelete("")]
        public async Task<IActionResult> Delete([FromBody] RemovePackingList command)
        {
            await _handler.RemovePackingListAsync(command);
            return Ok();
        }
    }
}
