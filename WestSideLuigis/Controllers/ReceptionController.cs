using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WestSideLuigis.Models;
using WestSideLuigis.Models.ViewModels;

namespace WestSideLuigis.Controllers
{
    public class ReceptionController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static ReceptionController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44334/api/");
        }



        // GET: Reception/List
        public ActionResult List()
        {
            //objective: communicate with my Reception data api to retreive a list of Receptions
            //curl https://localhost:44300/api/Receptiondata/listReceptions


            string url = "Receptiondata/listReceptions";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<ReceptionDto> Receptions = response.Content.ReadAsAsync<IEnumerable<ReceptionDto>>().Result;
            //Debug.WriteLine("Number of Receptions received ");
            //Debug.WriteLine(Receptions.Count());

            return View(Receptions);
        }

        // GET: Reception/Details/5
        public ActionResult View(int id)
        {

            DetailsReception ViewModel = new DetailsReception();

            //objective: communicate with my Reception data api to retreive one reception
            //curl https://localhost:44300/api/Receptiondata/findReception/{id}


            string url = "Receptiondata/findreception/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            ReceptionDto selectedreception = response.Content.ReadAsAsync<ReceptionDto>().Result;
            //Debug.WriteLine("Chosen Reception: ");
            //Debug.WriteLine(selectedreception.ReceptionName);

            ViewModel.SelectedReception = selectedreception;

            //show all attendees who signed up for this event

            url = "attendeedata/listattendeesforreception/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<AttendeeDto> RegAttendees = response.Content.ReadAsAsync<IEnumerable<AttendeeDto>>().Result;


            ViewModel.RegAttendees = RegAttendees;

            url = "attendeedata/ListAttendeesNotSignedUpForReception/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<AttendeeDto> PotenialAttendees = response.Content.ReadAsAsync<IEnumerable<AttendeeDto>>().Result;

            ViewModel.PotenialAttendees = PotenialAttendees;


            return View(ViewModel);
        }

        //POST: Reception/Add/{id}
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public ActionResult Add(int id, int attendeeid)
        {
            Debug.WriteLine("Attempting to associate reception id" + id + " with attendeeid " + attendeeid);

            string url = "attendeedata/AddAttendeeToReception/" + id + "/" + attendeeid;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("View/" + id);
        }


        //GET: Reception/Remove/{id}?AttendeeID={attendeeid}
        [HttpGet]
        [Authorize(Roles = "Admin")]

        public ActionResult Remove(int id, int attendeeid)
        {
            Debug.WriteLine("Attempting to remove reception id" + id + " with attendeeid " + attendeeid);

            string url = "attendeedata/RemoveAttendeeFromReception/" + id + "/" + attendeeid;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("View/" + id);
        }

        public ActionResult Errors()
        {
            return View();
        }

        // GET: Reception/New
        [Authorize(Roles = "Admin")]

        public ActionResult New()
        {
            return View();
        }

        // POST: Reception/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public ActionResult Create(Reception reception)
        {
            Debug.WriteLine("The name of the inputted reception is ");
            Debug.WriteLine(reception.ReceptionName);

            string url = "Receptiondata/addreception";


            string jsonpayload = jss.Serialize(reception);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");

            }
            else
            {
                return RedirectToAction("Errors");
            }


        }

        // GET: Reception/Edit/5
        [Authorize(Roles = "Admin")]

        public ActionResult Edit(int id)
        {
            string url = "Receptiondata/findreception/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ReceptionDto selectedreception = response.Content.ReadAsAsync<ReceptionDto>().Result;
            return View(selectedreception);
        }

        // POST: Reception/Update/5
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public ActionResult Update(int id, Reception reception)
        {
            string url = "Receptiondata/updatereception/" + id;
            string jsonpayload = jss.Serialize(reception);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Errors");
            }
        }

        // GET: Reception/DeleteConfirm/5
        [Authorize(Roles = "Admin")]

        public ActionResult DeleteConfirm(int id)
        {
            string url = "Receptiondata/findreception/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ReceptionDto selectedreception = response.Content.ReadAsAsync<ReceptionDto>().Result;
            return View(selectedreception);
        }

        // POST: Reception/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public ActionResult Delete(int id, Reception reception)
        {
            string url = "Receptiondata/deletereception/" + id;
            string jsonpayload = jss.Serialize(reception);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Errors");
            }
        }
    }
}
