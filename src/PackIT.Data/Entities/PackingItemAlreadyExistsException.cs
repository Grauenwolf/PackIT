﻿using PackIT.Shared.Exceptions;

namespace PackIT.Data.Entities
{
    public class PackingItemAlreadyExistsException : BadRequestException
    {
        public string ListName { get; }
        public string ItemName { get; }

        public PackingItemAlreadyExistsException(string listName, string itemName)
            : base($"Packing list: '{listName}' already defined item '{itemName}'")
        {
            ListName = listName;
            ItemName = itemName;
        }
    }
}
