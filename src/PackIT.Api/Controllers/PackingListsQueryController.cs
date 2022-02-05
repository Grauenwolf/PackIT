﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PackIT.Application.DTO;
using PackIT.Application.Queries;
using PackIT.Infrastructure.EF.Queries.Handlers;

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

        [HttpGet("{id:guid}")]
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
