using System;
using PackIT.Application.DTO;
using PackIT.Shared.Queries;

namespace PackIT.Application.Queries
{
    public class GetPackingList : IQuery<PackingListDto>
    {
        public Guid Id { get; set; }
    }
}
