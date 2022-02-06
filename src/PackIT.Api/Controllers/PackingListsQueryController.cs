using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PackIT.Infrastructure.Queries;
using PackIT.Infrastructure.Queries.Models;

namespace PackIT.Api.Controllers
{
    [Route("/api/PackingLists")]
    public class PackingListsQueryController : BaseController
    {
        private readonly PackingListQueryService _handler;

        public PackingListsQueryController(PackingListQueryService handler)
        {
            _handler = handler;
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<PackingListDto>> Get([FromRoute] GetPackingList query)
        {
            var result = await _handler.HandleAsync(query);
            return OkOrNotFound(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PackingListDto>>> Get([FromQuery] SearchPackingLists query)
        {
            var result = await _handler.HandleAsync(query);
            return OkOrNotFound(result);
        }


    }
}
