using System;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShopBridge.Models;
using System.IO;

namespace ShopBridge.Controllers
{
    public class ItemController : Controller
    {
        private ShopBridgeContext _context = new ShopBridgeContext();
        IItemRepository _itemRepository;

        public ItemController() : this(new ItemRepository()) { }

        // Instantiate the repository by using 'IItemRepository' interface
        // By using dependency injection (DI), our item repository is injected into a Itemcontroller's constructor
        public ItemController(IItemRepository iItemRepository)
        {
            _itemRepository = iItemRepository;
        }
        public ViewResult Index()
        {
            ViewData["ControllerName"] = this.ToString();
            string msg = TempData["Msg"] + string.Empty;
            ViewBag.Msg = msg;
            return View("Index", _itemRepository.GetItems());
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(400);
            }
            Item item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        [HttpGet]
        public ActionResult CreateNew()
        {
            Item item = new Item();
            item.Image = null;
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Item item, HttpPostedFileBase file)
        {
            try
            {
                string Msg = "";
                if (ModelState.IsValid && validate(item, ref Msg) == true)
                {
                    if (file != null)
                    {
                        byte[] bytes;
                        using (System.IO.BinaryReader br = new BinaryReader(file.InputStream))
                        {
                            bytes = br.ReadBytes(file.ContentLength);
                        }
                        item.Image = bytes;
                    }
                    _context.Items.Add(item);
                    _itemRepository.InsertItem(item);
                    TempData["Msg"] = "Item Added Successfully!";
                    return RedirectToAction("Index");
                }

                if (file != null)
                {
                    byte[] bytes;
                    using (BinaryReader br = new BinaryReader(file.InputStream))
                    {
                        bytes = br.ReadBytes(file.ContentLength);
                    }
                    item.Image = bytes;
                }
                ViewBag.Msg = Msg;
                return View("CreateNew", item);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex);
                ViewData["CreateError"] = "Item not added, Some Error Occured.";
            }
            return View("CreateNew");
        }

        private bool validate(Item item, ref string retMsg)
        {
            bool rflag = true;
            if (String.IsNullOrWhiteSpace(item.Name) == true)
            {
                retMsg = "Item Name is Compulsory!";
                rflag = false;
            }
            if (String.IsNullOrWhiteSpace(item.Description) == true)
            {
                retMsg = "Item Description is Compulsory!";
                rflag = false;
            }
            if (item.Price.Equals(null))
            {
                retMsg = "Item Price is Compulsory!";
                rflag = false;
            }

            return rflag;
        }

        // GET: Items/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewData["ControllerName"] = this.ToString();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = _context.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Item item, HttpPostedFileBase file)
        {
            string Msg = "";
            if (ModelState.IsValid && validate(item, ref Msg) == true)
            {
                try
                {
                    using (ShopBridgeContext db = new ShopBridgeContext())
                    {
                        var ID = db.Items.Find(item.Id);
                        ID.Name = item.Name;
                        ID.Description = item.Description;
                        ID.Price = item.Price;
                        if (file != null)
                        {
                            byte[] bytes;
                            using (BinaryReader br = new BinaryReader(file.InputStream))
                            {
                                bytes = br.ReadBytes(file.ContentLength);
                            }
                            ID.Image = bytes;
                        }
                        db.SaveChanges();
                    }
                    TempData["Msg"] = "Item Updated Successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex) { ModelState.AddModelError("", "Exception Occured"); }
                finally { }
            }
            return View(item);
        }

        // GET: Items/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Item item = await _context.Items.FindAsync(id);
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            TempData["Msg"] = "Item Removed Successfully!";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
