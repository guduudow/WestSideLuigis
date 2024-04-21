using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using WestSideLuigis.Models;
namespace WestSideLuigis.Controllers
{
    /// <summary>
    /// Controller for managing orders data.
    /// </summary>
    public class OrdersDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Retrieves a list of all orders.
        /// </summary>
        /// <returns>An IHttpActionResult containing the list of orders.</returns>
        [HttpGet]
        [Route("api/OrderData/List")]
        public IHttpActionResult List()
        {
            var orders = db.Orders.ToList();
            return Ok(orders);
        }
        /// <summary>
        /// Retrieves a list of menu items for a specific order.
        /// </summary>
        /// <param name="orderId">The ID of the order.</param>
        /// <returns>An IHttpActionResult containing the list of menu items associated with the order.</returns>
        [HttpGet]
        [Route("api/OrderData/ListMenuItemsForOrder/{orderId}")]
        public IHttpActionResult ListMenuItemsForOrder(int orderId)
        {
            var order = db.Orders.Include(o => o.MenuItems).FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
            {
                return NotFound();
            }
            var menuItems = order.MenuItems.Select(item => new MenuItemDto
            {
                ItemID = item.ItemID,
                ItemName = item.ItemName,
                ItemDescription = item.ItemDescription,
                Price = item.Price,
                CategoryName = item.Category.CategoryName
            });
            return Ok(menuItems);
        }

        /// <summary>
        /// Retrieves a list of orders containing a specific menu item.
        /// </summary>
        /// <param name="menuItemId">The ID of the menu item.</param>
        /// <returns>An IHttpActionResult containing the list of orders containing the specified menu item.</returns>
        [HttpGet]
        [Route("api/OrderData/ListOrdersForMenuItem/{menuItemId}")]
        public IHttpActionResult ListOrdersForMenuItem(int menuItemId)
        {
            var orders = db.Orders.Include(o => o.MenuItems).Where(o => o.MenuItems.Any(item => item.ItemID == menuItemId));
            if (orders == null)
            {
                return NotFound();
            }
            var orderDtos = orders.Select(o => new OrderDto
            {
                OrderId = o.OrderId,
                TableNo = o.TableNo,
                OrderDate = o.OrderDate,
                OrderStatus = o.OrderStatus
            });
            return Ok(orderDtos);
        }

        /// <summary>
        /// Adds a menu item to a specific order.
        /// </summary>
        /// <param name="orderId">The ID of the order.</param>
        /// <param name="menuItemId">The ID of the menu item to add.</param>
        /// <returns>An IHttpActionResult indicating success or failure.</returns>
        [HttpPost]
        [Route("api/OrderData/AddMenuItemToOrder/{orderId}/{menuItemId}")]
        [Authorize(Roles = "Admin")]

        public IHttpActionResult AddMenuItemToOrder(int orderId, int menuItemId)
        {
            var order = db.Orders.Find(orderId);
            if (order == null)
            {
                return NotFound();
            }

            var menuItem = db.MenuItems.Find(menuItemId);
            if (menuItem == null)
            {
                return BadRequest("Invalid menu item ID.");
            }

            order.MenuItems.Add(menuItem);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Removes a menu item from a specific order.
        /// </summary>
        /// <param name="orderId">The ID of the order.</param>
        /// <param name="menuItemId">The ID of the menu item to remove.</param>
        /// <returns>An IHttpActionResult indicating success or failure.</returns>
        [HttpPost]
        [Route("api/OrderData/RemoveMenuItemFromOrder/{orderId}/{menuItemId}")]
        [Authorize(Roles = "Admin")]

        public IHttpActionResult RemoveMenuItemFromOrder(int orderId, int menuItemId)
        {
            var order = db.Orders.Find(orderId);
            if (order == null)
            {
                return NotFound();
            }

            var menuItem = db.MenuItems.Find(menuItemId);
            if (menuItem == null)
            {
                return BadRequest("Invalid menu item ID.");
            }

            order.MenuItems.Remove(menuItem);
            db.SaveChanges();

            return Ok();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
