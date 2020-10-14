using ShopBridge.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShopBridge.Tests.Models
{
    class InmemoryItemRepository : IItemRepository
    {
        private List<Item> _context = new List<Item>();

        public Exception ExceptionToThrow
        {
            get;
            set;
        }

        public IEnumerable<Item> GetItems()
        {
            return _context.ToList();
        }

        public Item GetItemByID(int id)
        {
            return _context.FirstOrDefault(d => d.Id  == id);
        }

        public void InsertItem(Item item)
        {
            if (ExceptionToThrow != null)
                throw ExceptionToThrow;
            _context.Add(item);
        }

        public void SaveChanges(Item ItemToUpdate)
        {
            foreach (Item Item in _context)
            {
                if (Item.Id == ItemToUpdate.Id)
                {
                    _context.Remove(Item);
                    _context.Add(ItemToUpdate);
                    break;
                }
            }
        }

        public void Add(Item ItemToAdd)
        {
            _context.Add(ItemToAdd);
        }

        public int SaveChanges()
        {
            return 1;
        }

        public void RemoveItem(int id)
        {
            throw new NotImplementedException();
        }

    }
}
