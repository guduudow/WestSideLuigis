using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WestSideLuigis.Models;

namespace WestSideLuigis.Controllers
{
    public class MenuItemDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // List MenuItem
        // GET: api/MenuItemData/ListMenuItem
        /// <summary>
        /// Retrieves a list of menu items.
        /// </summary>
        /// <returns>A list of menu items.</returns>
        [HttpGet]
        [Route("api/MenuItemData/ListMenuItem")]
        public IEnumerable<MenuItemDto> ListMenuItem()
        {
            // Sending a query to the database
            List<MenuItem> MenuItems = db.MenuItems.ToList();

            List<MenuItemDto> MenuItemDtos = new List<MenuItemDto>();
            MenuItems.ForEach(b => MenuItemDtos.Add(new MenuItemDto()
            {
                ItemID = b.ItemID,
                ItemName = b.ItemName,
                ItemDescription = b.ItemDescription,
                Price = b.Price,
                CategoryName = b.Category.CategoryName,
                BeerID = b.Beer.BeerID,
                BeerName = b.Beer.BeerName
            }
            ));

            // Read
            return MenuItemDtos;
        }
        // Find MenuItem
        // Get: api/MenuItemData/FindMenuItem/{id}
        /// <summary>
        /// Finds a specific menu item by its ID.
        /// </summary>
        /// <param name="id">The ID of the menu item to find.</param>
        /// <returns>The menu item with the specified ID.</returns>
        [ResponseType(typeof(MenuItem))]
        [HttpGet]
        public IHttpActionResult FindMenuItem(int id)
        {
            MenuItem MenuItem = db.MenuItems.Find(id);
            MenuItemDto MenuItemDto = new MenuItemDto()
            {
                ItemID = MenuItem.ItemID,
                ItemName = MenuItem.ItemName,
                ItemDescription = MenuItem.ItemDescription,
                Price = MenuItem.Price,
                CategoryName = MenuItem.Category.CategoryName,
                BeerID = MenuItem.Beer.BeerID,
                BeerName = MenuItem.Beer.BeerName
            };
            if (MenuItem == null)
            {
                return NotFound();
            }

            return Ok(MenuItemDto);
        }

        // Add MenuItem
        // POST: api/MenuData/AddMenuItem
        /// <summary>
        /// Adds a new menu item to the database.
        /// </summary>
        /// <param name="menuitem">The menu item to add.</param>
        /// <returns>The added menu item.</returns>
        [ResponseType(typeof(MenuItem))]
        [HttpPost]
        public IHttpActionResult AddMenuItem(MenuItem menuitem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MenuItems.Add(menuitem);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = menuitem.ItemID }, menuitem);
        }

        // Update MenuItem
        // POST: api/MenuItemData/Update/5
        /// <summary>
        /// Updates an existing menu item in the database.
        /// </summary>
        /// <param name="id">The ID of the menu item to update.</param>
        /// <param name="menuitem">The updated menu item.</param>
        /// <returns>No content.</returns>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult Update(int id, MenuItem menuitem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != menuitem.ItemID)
            {
                return BadRequest();
            }

            db.Entry(menuitem).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MenuItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // Delete MenuItem
        // POST: api/MenuData/DeleteMenuItem/5
        /// <summary>
        /// Deletes a menu item from the database.
        /// </summary>
        /// <param name="id">The ID of the menu item to delete.</param>
        /// <returns>OK if successful.</returns>
        [ResponseType(typeof(MenuItem))]
        [HttpPost]
        [Route("api/MenuItemData/DeleteMenuItem/{id}")]
        public IHttpActionResult DeleteMenuItem(int id)
        {
            MenuItem menuitem = db.MenuItems.Find(id);
            if (menuitem == null)
            {
                return NotFound();
            }

            db.MenuItems.Remove(menuitem);
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

        private bool MenuItemExists(int id)
        {
            return db.MenuItems.Count(e => e.ItemID == id) > 0;
        }
    }
}