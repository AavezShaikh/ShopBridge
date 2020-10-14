using System.Collections.Generic;
using System.Linq;

namespace ShopBridge.Models
{
    // The ItemRepository pattern is intended to create an abstraction layer between the data access layer 
    // and the business logic layer of a shopBridge application. 
    // For the Item entity type, repository interface (IItemRepository) and a repository class (ItemRepository) are created

    public class ItemRepository : IItemRepository
    {
        private ShopBridgeContext _context = new ShopBridgeContext();

        public IEnumerable<Item> GetItems()
        {
            return _context.Items.ToList();
        }
        public Item GetItemByID(int id)
        {
            return _context.Items.FirstOrDefault(d => d.Id == id);
        }
        public void InsertItem(Item item)
        {
            _context.Items.Add(item);
            _context.SaveChanges();
        }
        public void RemoveItem(int id)
        {
            var conToDel = GetItemByID(id);
            _context.Items.Remove(conToDel);
            _context.SaveChanges();

        }
        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }

    // This IItemRepository interface declares a typical set of CRUD methods, 
    // including two read methods; one that returns all Item entity sets, and one that finds a single Item entity by ID.
    public interface IItemRepository
    {
        IEnumerable<Item> GetItems();
        Item GetItemByID(int id);
        void InsertItem(Item item);
        void RemoveItem(int id);
        int SaveChanges();
    }
}
