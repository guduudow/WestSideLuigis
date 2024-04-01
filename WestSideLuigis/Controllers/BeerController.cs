using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using WestSideLuigis.Models;
using WestSideLuigis.Models.ViewModels;
using System.Web.Script.Serialization;


namespace WestSideLuigis.Controllers
{
    public class BeerController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static BeerController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44393/api/");
        }

        /// <summary>
        /// Gets a list of beers from Beer API and renders them in the view.
        /// </summary>
        /// <returns>
        /// list of beers
        /// </returns>
        /// <example>
        /// GET: Beer/List
        /// </example>

        // GET: Beer/List
        public ActionResult List()
        {
            //goal is to communicate with beer api to retrieve a list of beers
            //curl https://localhost:44393/api/beerdata/listbeers

            string url = "beerdata/listbeers";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is: ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<BeerDto> beers = response.Content.ReadAsAsync<IEnumerable<BeerDto>>().Result;

            //Debug.WriteLine("Number of beers received: ");
            //Debug.WriteLine(beers.Count());

            return View(beers);
        }

        /// <summary>
        /// Gets the details of a specific beer from the Beer API by its ID and renders it in the view
        /// </summary>
        /// <param name="id">id used to identify specific beer in database</param>
        /// <returns>
        /// View of specific beer
        /// </returns>
        /// <example>
        /// GET: Beer/Details/3
        /// </example>

        // GET: Beer/Details/3
        public ActionResult Details(int id)
        {
            //goal is to communicate with beer api to retrieve one beer
            //curl https://localhost:44393/api/beerdata/findbeer{id}

            UpdateBeer ViewModel = new UpdateBeer();

            string url = "beerdata/findbeer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is: ");
            //Debug.WriteLine(response.StatusCode);

            BeerDto SelectedBeer = response.Content.ReadAsAsync<BeerDto>().Result;

            ViewModel.SelectedBeer = SelectedBeer;
            //Debug.WriteLine("Beer recieved: ");
            //Debug.WriteLine(SelectedBeer.BeerName);
            //Debug.WriteLine(SelectedBeer.BreweryName);

            return View(ViewModel);
        }
        /// <summary>
        /// Displays a view if there is an error
        /// </summary>
        /// <returns>
        /// View of an error page if there is an error
        /// </returns>
        /// <example>
        /// GET: Beer/Error
        /// </example>
        public ActionResult Error()
        {

            return View();
        }

        /// <summary>
        /// Displays a form for creating a new beer.
        /// </summary>
        /// <returns>
        /// view for creating a new beer
        /// </returns>
        /// <example>
        /// GET: Beer/New
        /// </example>

        // GET: Beer/New
        public ActionResult New()
        {
            //info about all breweries in the database
            //GET api/brewerydata/listbreweries

            string url = "brewerydata/listbreweries";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<BreweryDto> BreweryOptions = response.Content.ReadAsAsync<IEnumerable<BreweryDto>>().Result;

            return View(BreweryOptions);
        }

        /// <summary>
        /// Creates a new beer in the database using data from the form
        /// </summary>
        /// <param name="beer">Beer object which has the details of the new beer thats to be added</param>
        /// <returns>
        /// Redirects to the beer list if the creation is successful, else it returns to an error view
        /// </returns>
        /// <example>
        /// POST: Beer/Create
        /// </example>

        // POST: Beer/Create
        [HttpPost]
        public ActionResult Create(Beer beer)
        {
            Debug.WriteLine("the json payload is: ");
            Debug.WriteLine(beer.BeerName);

            //goal: add a new beer to our system using the API
            //curl -H "Content-Type:application/json" -d @beer.json https://localhost:44393/api/beerdata/addbeer
            string url = "beerdata/addbeer";


            string jsonpayload = jss.Serialize(beer);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("list");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        /// <summary>
        /// displays view with form to edit a specific beer
        /// </summary>
        /// <param name="id">id for specific beer thats to be edited</param>
        /// <returns>
        /// View with form to edit specific beers details
        /// </returns>
        /// <example>
        /// GET: Beer/Edit/3
        /// </example>

        // GET: Beer/Edit/3
        public ActionResult Edit(int id)
        {
            UpdateBeer ViewModel = new UpdateBeer();

            //the existing beer info
            string url = "beerdata/findbeer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            BeerDto SelectedBeer = response.Content.ReadAsAsync<BeerDto>().Result;
            ViewModel.SelectedBeer = SelectedBeer;

            //Need to include all breweries available when updating the beer
            //the existing beer info
            url = "brewerydata/listbreweries/";
            response = client.GetAsync(url).Result;
            IEnumerable<BreweryDto> BreweryOptions = response.Content.ReadAsAsync<IEnumerable<BreweryDto>>().Result;

            ViewModel.BreweryOptions = BreweryOptions;

            return View(ViewModel);
        }

        /// <summary>
        /// Updates details of a specific beer in the database based on the id
        /// </summary>
        /// <param name="id">id of the beer thats being updated</param>
        /// <param name="beer">beer object with the updated details of the beer</param>
        /// <returns>
        /// on succeddful update it redirects to beer list, else redirects to error page
        /// </returns>
        /// <example>
        /// POST: Beer/Update/3
        /// </example>

        // POST: Beer/Update/3
        [HttpPost]
        public ActionResult Update(int id, Beer beer, HttpPostedFileBase BeerPic)
        {
            string url = "beerdata/updatebeer/" + id;
            string jsonpayload = jss.Serialize(beer);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode && BeerPic != null)
            {
                //Updaying the beer picture as a separate request
                Debug.WriteLine("Calling Update Image method.");
                //Send over image data 
                url = "BeerData/UploadBeerPic/" + id;
                Debug.WriteLine("Recieved beer picture " + BeerPic.FileName);

                MultipartFormDataContent requestcontent = new MultipartFormDataContent();
                HttpContent imagecontent = new StreamContent(BeerPic.InputStream);
                requestcontent.Add(imagecontent, "BeerPic", BeerPic.FileName);
                response = client.PostAsync(url, requestcontent).Result;

                return RedirectToAction("List");
            }
            else if (response.IsSuccessStatusCode)
            {
                //no img upload but update still works
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// Displays view for confirmation on deleting a specific beer
        /// </summary>
        /// <param name="id">id for specific beer</param>
        /// <returns>
        /// view for confirming deletion of specified beer
        /// </returns>
        /// <example>
        /// GET: Beer/Delete/3
        /// </example>

        // GET: Beer/Delete/3
        public ActionResult DeleteConfirm(int id)
        {
            string url = "beerdata/findbeer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            BeerDto selectedbeer = response.Content.ReadAsAsync<BeerDto>().Result;

            return View(selectedbeer);
        }

        /// <summary>
        /// deletes a specified beer from the database using its id
        /// </summary>
        /// <param name="id">id for specific beer</param>
        /// <returns>
        /// redirects to beer list if the deletion was successful, else redirects to error page
        /// </returns>
        /// <example>
        /// POST: Beer/Delete/5
        /// </example>

        // POST: Beer/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "beerdata/deletebeer/" + id;
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
