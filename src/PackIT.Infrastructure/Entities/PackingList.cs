using System.Collections.Generic;
using System.Linq;
using PackIT.Infrastructure.Events;
using PackIT.Infrastructure.Exceptions;
using PackIT.Infrastructure.ValueObjects;
using PackIT.Shared.Domain;

namespace PackIT.Infrastructure.Entities
{
    public class PackingList : AggregateRoot<PackingListId>
    {
        public PackingListName Name { get; init; }
        public Localization Localization { get; init; }

        private readonly LinkedList<PackingItem> _items = new();

        private PackingList(PackingListId id, PackingListName name, Localization localization, LinkedList<PackingItem> items)
            : this(id, name, localization)
        {
            _items = items;
        }

        private PackingList()
        {
        }

        internal PackingList(PackingListId id, PackingListName name, Localization localization)
        {
            Id = id;
            Name = name;
            Localization = localization;
        }

        public void AddItem(PackingItem item)
        {
            var alreadyExists = _items.Any(i => i.Name == item.Name);

            if (alreadyExists)
            {
                throw new PackingItemAlreadyExistsException(Name, item.Name);
            }

            _items.AddLast(item);
            AddEvent(new PackingItemAdded(this, item));
        }

        public void AddItems(IEnumerable<PackingItem> items)
        {
            foreach (var item in items)
            {
                AddItem(item);
            }
        }

        public void PackItem(string itemName)
        {
            var item = GetItem(itemName);
            var packedItem = item with { IsPacked = true };

            _items.Find(item).Value = packedItem;
            AddEvent(new PackingItemPacked(this, item));
        }

        public void RemoveItem(string itemName)
        {
            var item = GetItem(itemName);
            _items.Remove(item);
            AddEvent(new PackingItemRemoved(this, item));
        }

        private PackingItem GetItem(string itemName)
        {
            var item = _items.SingleOrDefault(i => i.Name == itemName);

            if (item is null)
            {
                throw new PackingItemNotFoundException(itemName);
            }

            return item;
        }
    }
}
