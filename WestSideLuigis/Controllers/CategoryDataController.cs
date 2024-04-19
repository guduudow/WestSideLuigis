using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http.Description;
using System.Web.Http;
using System.Web.Mvc;
using WestSideLuigis.Models;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using AuthorizeAttribute = System.Web.Http.AuthorizeAttribute;

namespace WestSideLuigis.Controllers
{
    /// <summary>
    /// Controller for managing categories data.
    /// </summary>
    public class CategoryDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Gets a list of all categories.
        /// </summary>
        /// <returns>An IEnumerable of CategoryDto objects representing categories.</returns>
        [HttpGet]
        [Route("api/CategoryData/ListCategory")]
        public IEnumerable<CategoryDto> ListCategory()
        {
            // Retrieving categories from the database
            List<Category> Categories = db.Categories.ToList();

            List<CategoryDto> CategoryDtos = new List<CategoryDto>();
            Categories.ForEach(b => CategoryDtos.Add(new CategoryDto()
            {
                CategoryId = b.CategoryId,
                CategoryName = b.CategoryName,
            }));

            return CategoryDtos;
        }

        /// <summary>
        /// Finds a category by ID.
        /// </summary>
        /// <param name="id">The ID of the category to find.</param>
        /// <returns>An IHttpActionResult containing the found category or a NotFound response if not found.</returns>
        [ResponseType(typeof(Category))]
        [HttpGet]
        public IHttpActionResult FindCategory(int id)
        {
            Category Category = db.Categories.Find(id);
            CategoryDto CategoryDto = new CategoryDto()
            {
                CategoryId = Category.CategoryId,
                CategoryName = Category.CategoryName,
            };
            if (Category == null)
            {
                return NotFound();
            }

            return Ok(CategoryDto);
        }

        /// <summary>
        /// Adds a new category.
        /// </summary>
        /// <param name="Category">The category object to add.</param>
        /// <returns>An IHttpActionResult containing the added category.</returns>
        [ResponseType(typeof(Category))]
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public IHttpActionResult AddCategory(Category Category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Categories.Add(Category);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Category.CategoryId }, Category);
        }

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="id">The ID of the category to update.</param>
        /// <param name="Category">The updated category object.</param>
        /// <returns>An IHttpActionResult indicating success or failure.</returns>
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public IHttpActionResult UpdateCategory(int id, Category Category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Category.CategoryId)
            {
                return BadRequest();
            }

            db.Entry(Category).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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

        /// <summary>
        /// Deletes a category.
        /// </summary>
        /// <param name="id">The ID of the category to delete.</param>
        /// <returns>An IHttpActionResult indicating success or failure.</returns>
        [ResponseType(typeof(Category))]
        [HttpPost]
        [Route("api/CategoryData/DeleteCategory/{id}")]
        [Authorize(Roles = "Admin")]

        public IHttpActionResult DeleteCategory(int id)
        {
            Category Category = db.Categories.Find(id);
            if (Category == null)
            {
                return NotFound();
            }

            db.Categories.Remove(Category);
            db.SaveChanges();

            return Ok();
        }
        /// <summary>
        /// Retrieves a list of menu items for a specific category.
        /// </summary>
        /// <param name="categoryId">The ID of the category.</param>
        /// <returns>An IHttpActionResult containing the list of menu items associated with the category.</returns>
        [HttpGet]
        [Route("api/CategoryData/ListMenuItemsForCategory/{categoryId}")]
        public IHttpActionResult ListMenuItemsForCategory(int categoryId)
        {
            Debug.WriteLine("CATEGORY DATA CONTROLLER" + categoryId);
            var menuItems = db.MenuItems.Where(item => item.CategoryId == categoryId)
                                        .Select(item => new MenuItemDto
                                        {
                                            ItemID = item.ItemID,
                                            ItemName = item.ItemName,
                                            ItemDescription = item.ItemDescription,
                                            Price = item.Price,
                                            CategoryName = item.Category.CategoryName
                                        });
            return Ok(menuItems);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategoryExists(int id)
        {
            return db.Categories.Count(e => e.CategoryId == id) > 0;
        }
    }
}