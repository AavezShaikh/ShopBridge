using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopBridge.Controllers;
using ShopBridge.Models;
using ShopBridge.Tests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ShopBridge.Tests.Controllers
{
    [TestClass]
    public class ItemTestContollerTest
    {
        [TestMethod]
        public void IndexView()
        {
            var itemController = GetItemController(new InmemoryItemRepository());
            ViewResult result = itemController.Index();
            Assert.AreEqual("Index", result.ViewName);
        }
        private static ItemController GetItemController(IItemRepository Itemrepository)
        {
            ItemController Itemcontroller = new ItemController(Itemrepository);
            Itemcontroller.ControllerContext = new ControllerContext()
            {
                Controller = Itemcontroller,
                RequestContext = new RequestContext(new MockHttpContext(), new RouteData())
            };
            return Itemcontroller;
        }

        private class MockHttpContext : HttpContextBase
        {
            private readonly IPrincipal _user = new GenericPrincipal(new GenericIdentity("someUser"), null /* roles */);

            public override IPrincipal User
            {
                get
                {
                    return _user;
                }
                set
                {
                    base.User = value;
                }
            }
        }

        [TestMethod]
        public void Create_PostItemInRepository()
        {
            InmemoryItemRepository itemrepository = new InmemoryItemRepository();
            ItemController empcontroller = GetItemController(itemrepository);
            Item item = GetItemID();
            empcontroller.Create(item, null);
            IEnumerable<Item> items = itemrepository.GetItems();
            Assert.IsTrue(items.Contains(item));
        }

        [TestMethod]
        public void Create_PostRedirectOnSuccess()
        {
            ItemController controller = GetItemController(new InmemoryItemRepository());
            Item model = GetItemID();
            var result = (RedirectToRouteResult)controller.Create(model, null);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void ViewIsNotValid()
        {
            ItemController itemcontroller = GetItemController(new InmemoryItemRepository());
            itemcontroller.ModelState.AddModelError("", "Mock Error Message");
            Item model = GetItemName(1, "", "", 0, null);
            var result = (ViewResult)itemcontroller.Create(model, null);
            Assert.AreEqual("CreateNew", result.ViewName);
        }

        /// <summary>  
        ///  
        /// </summary>  
        [TestMethod]
        public void RepositoryThrowsException()
        {
            // Arrange  
            InmemoryItemRepository itemrepository = new InmemoryItemRepository();
            Exception exception = new Exception();
            itemrepository.ExceptionToThrow = exception;
            ItemController controller = GetItemController(itemrepository);
            Item item = GetItemID();
            var result = (ViewResult)controller.Create(item, null);
            //Assert
            Assert.AreEqual("CreateNew", result.ViewName);
            ModelState modelState = result.ViewData.ModelState[""];
            Assert.IsNotNull(modelState);
            Assert.IsTrue(modelState.Errors.Any());
            Assert.AreEqual(exception, modelState.Errors[0].Exception);
        }

        Item GetItemID()
        {
            return GetItemName(1, "Chair", "Chair", 550, null);
        }

        Item GetItemName(int IntId, string StringName, string StringDescription, decimal DecPrice, byte[] byteImage)
        {
            return new Item
            {
                Id = IntId,
                Name = StringName,
                Description = StringDescription,
                Price = DecPrice,
                Image = byteImage
            };
        }

        [TestMethod]
        public void GetAllEmployeeFromRepository()
        {
            // Arrange  
            Item item1 = GetItemName(1, "Chair", "Chair", 550, null);
            Item item2 = GetItemName(2, "Table", "Table", 1050, null);
            InmemoryItemRepository itemrepository = new InmemoryItemRepository();
            itemrepository.Add(item1);
            itemrepository.Add(item2);
            var controller = GetItemController(itemrepository);
            var result = controller.Index();
            var datamodel = (IEnumerable<Item>)result.ViewData.Model;
            CollectionAssert.Contains(datamodel.ToList(), item1);
            CollectionAssert.Contains(datamodel.ToList(), item2);
        }
    }
}