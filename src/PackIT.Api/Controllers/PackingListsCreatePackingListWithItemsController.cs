﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PackIT.Application.Commands;
using PackIT.Application.Commands.Handlers;

namespace PackIT.Api.Controllers
{
    [Route("/api/PackingLists")]
    public class PackingListsCreatePackingListWithItemsController : BaseController
    {
        private readonly CreatePackingListWithItemsService _handler;

        public PackingListsCreatePackingListWithItemsController(CreatePackingListWithItemsService handler)
        {
            _handler = handler;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreatePackingListWithItems command)
        {
            await _handler.HandleAsync(command);
            return CreatedAtAction(nameof(Post), new { id = command.Id }, null);
        }


    }
}
