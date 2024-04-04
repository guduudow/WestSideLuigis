using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using static System.Data.Entity.Infrastructure.Design.Executor;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using WestSideLuigis.Models;
using MenuItem = WestSideLuigis.Models.MenuItem;
using WestSideLuigis.Models.ViewModels;

namespace WestSideLuigis.Controllers
{
    public class MenuItemController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        // Static constructor to initialize HttpClient with base address
        static MenuItemController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44361/api/menuitemdata/");
        }

        // GET: Menu/List
        public ActionResult List()
        {
            // Get a list of menu items from the API
            string url = "ListMenuItem";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<MenuItemDto> menuitems = response.Content.ReadAsAsync<IEnumerable<MenuItemDto>>().Result;
            return View(menuitems);
        }

        // GET: Menu/Details/5
        public ActionResult Details(int id)
        {
            // Get details of a specific menu item
            DetailsMenu ViewModel = new DetailsMenu();
            string url = "FindMenuItem/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            MenuItemDto selectedmenu = response.Content.ReadAsAsync<MenuItemDto>().Result;
            ViewModel.selectedmenu = selectedmenu;
            return View(ViewModel);
        }

        // GET: Menu/Error
        public ActionResult Error()
        {
            // Error view
            return View();
        }

        // GET: Menu/New
        public ActionResult New()
        {
            // View for adding a new menu item
            return View();
        }

        // POST: Menu/Create
        [HttpPost]
        public ActionResult Create(MenuItem menuitem)
        {
            // Add a new menu item
            string url = "AddMenuItem";
            string jsonpayload = jss.Serialize(menuitem);
            HttpContent content = new StringContent(jsonpayload);
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

        // GET: Menu/Edit/5
        public ActionResult Edit(int id)
        {
            // View for editing a menu item
            DetailsMenu ViewModel = new DetailsMenu();
            string url = "FindMenuItem/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            MenuItemDto selectedmenu = response.Content.ReadAsAsync<MenuItemDto>().Result;
            ViewModel.selectedmenu = selectedmenu;
            return View(ViewModel);
        }

        // POST: Menu/Update/5
        [HttpPost]
        public ActionResult Update(int id, MenuItem menuitem)
        {
            // Update a menu item
            string url = "Update/" + id;
            string jsonPayload = jss.Serialize(menuitem);
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

        // GET: Menu/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            // Confirmation view for deleting a menu item
            string url = "FindMenuItem/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            MenuItemDto selectedmenu = response.Content.ReadAsAsync<MenuItemDto>().Result;
            return View(selectedmenu);
        }

        // POST: Menu/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            // Delete a menu item
            string url = "DeleteMenuItem/" + id;
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
