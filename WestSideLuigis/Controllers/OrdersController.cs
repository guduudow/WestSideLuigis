using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WestSideLuigis.Models;

namespace WestSideLuigis.Controllers
{
    public class OrdersController : Controller
    {
        private static readonly HttpClient client;

        // Static constructor to initialize HttpClient with base address
        static OrdersController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44361/api/orderdata/");
        }

        //GET: Orders/List
        public ActionResult List()
        {
            HttpResponseMessage response = client.GetAsync("List").Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<OrderDto> orders = response.Content.ReadAsAsync<IEnumerable<OrderDto>>().Result;
                return View(orders);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Orders/ListMenuItemsForOrder/5
        [Route("/Orders/MenuItemsForOrder/{orderId}")]
        public ActionResult MenuItemsForOrder(int orderId)
        {
            Debug.WriteLine(orderId);
            string url = "ListMenuItemsForOrder/" + orderId;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<MenuItemDto> menuItems = response.Content.ReadAsAsync<IEnumerable<MenuItemDto>>().Result;
                return View(menuItems);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Order/OrdersForMenuItem/5
        public ActionResult OrdersForMenuItem(int menuItemId)
        {
            string url = "ListOrdersForMenuItem/" + menuItemId;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                IEnumerable<OrderDto> orders = response.Content.ReadAsAsync<IEnumerable<OrderDto>>().Result;
                return View(orders);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Orders/AddMenuItemToOrder/5/10
        [HttpPost]
        public ActionResult AddMenuItemToOrder(int orderId, int menuItemId)
        {
            HttpResponseMessage response = client.PostAsync($"AddMenuItemToOrder/{orderId}/{menuItemId}", null).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ListMenuItemsForOrder", new { orderId = orderId });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Order/RemoveMenuItemFromOrder/5/10
        [HttpPost]
        public ActionResult RemoveMenuItemFromOrder(int orderId, int menuItemId)
        {
            HttpResponseMessage response = client.PostAsync($"RemoveMenuItemFromOrder/{orderId}/{menuItemId}", null).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ListMenuItemsForOrder", new { orderId = orderId });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Order/Error
        public ActionResult Error()
        {
            // Error view
            return View();
        }
    }
}