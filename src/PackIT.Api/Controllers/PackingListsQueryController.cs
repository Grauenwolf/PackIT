using System;
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

        [HttpGet("{id}")]
        public async Task<ActionResult<PackingListDto>> Get([FromRoute] Guid id)
        {
            var result = await _handler.GetById(id);
            return OkOrNotFound(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PackingListDto>>> Get([FromQuery] string searchPhrase)
        {
            var result = await _handler.Search(searchPhrase);
            return OkOrNotFound(result);
        }


    }
}
