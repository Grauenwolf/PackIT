using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PackIT.Infrastructure.Commands;
using PackIT.Infrastructure.Commands.Handlers;

namespace PackIT.Api.Controllers
{
    [Route("/api/PackingLists")]
    public class PackingListsCommandController : BaseController
    {
        private readonly PackingListCommandService _handler;

        public PackingListsCommandController(PackingListCommandService handler)
        {
            _handler = handler;
        }

        [HttpPut("{packingListId}/items")]
        public async Task<IActionResult> Put([FromBody] AddPackingItem command)
        {
            await _handler.HandleAsync(command);
            return Ok();
        }

        [HttpPut("{packingListId:guid}/items/{name}/pack")]
        public async Task<IActionResult> Put([FromBody] PackItem command)
        {
            await _handler.HandleAsync(command);
            return Ok();
        }

        [HttpDelete("{packingListId:guid}/items/{name}")]
        public async Task<IActionResult> Delete([FromBody] RemovePackingItem command)
        {
            await _handler.HandleAsync(command);
            return Ok();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromBody] RemovePackingList command)
        {
            await _handler.HandleAsync(command);
            return Ok();
        }
    }
}
