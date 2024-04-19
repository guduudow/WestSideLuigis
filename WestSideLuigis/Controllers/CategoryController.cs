using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using WestSideLuigis.Models.ViewModels;
using WestSideLuigis.Models;

namespace WestSideLuigis.Controllers
{
    public class CategoryController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static CategoryController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44334/api/categorydata/");
        }

        // GET: Category/List
        public ActionResult List()
        {
            // Retrieve a list of categories from the Category data API
            string url = "ListCategory";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<CategoryDto> categories = response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;
            return View(categories);
        }

        // GET: Category/Error
        public ActionResult Error()
        {
            return View();
        }

        // GET: Category/New
        [Authorize(Roles = "Admin")]

        public ActionResult New()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public ActionResult Create(Category category)
        {
            // Add a new category to the Category data
            string url = "AddCategory";
            string jsonPayload = jss.Serialize(category);

            HttpContent content = new StringContent(jsonPayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Category/Details/5
        public ActionResult Details(int id)
        {
            // Get details of a specific category item
            DetailsMenu ViewModel = new DetailsMenu();
            string url = "ListMenuItemsForCategory/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            var responseContent = response.Content.ReadAsStringAsync().Result;
            var selectedCategory = JsonConvert.DeserializeObject<IEnumerable<MenuItemDto>>(responseContent);
            var menus = JsonConvert.DeserializeObject<IEnumerable<MenuItemDto>>(responseContent);

            ViewModel.selectedcategory = selectedCategory.FirstOrDefault();
            ViewModel.MenuItems = menus;
            Debug.WriteLine("Response Content" + responseContent);
            return View(ViewModel);
        }


        // GET: Category/DeleteConfirm/5
        [Authorize(Roles = "Admin")]

        public ActionResult DeleteConfirm(int id)
        {
            // Retrieve category details for deletion confirmation
            string url = "FindCategory/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CategoryDto selectedCategory = response.Content.ReadAsAsync<CategoryDto>().Result;

            return View(selectedCategory);
        }

        // POST: Category/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public ActionResult Delete(int id)
        {
            // Delete a category
            string url = "DeleteCategory/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}