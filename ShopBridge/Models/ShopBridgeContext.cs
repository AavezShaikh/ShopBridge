using System.Data.Entity;
namespace ShopBridge.Models
{
    public class ShopBridgeContext : DbContext
    {
        //public ShopBridgeContext()
        //    : base("name=ShopBridgeContext")
        //{
        //}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<ShopBridgeContext>(null);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Item> Items { get; set; }
    }
}