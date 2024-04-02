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
    public class AttendeeController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static AttendeeController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44300/api/");
        }


        // GET: Attendee/List
        public ActionResult List()
        {
            //objective: communicate with my attendee data api to retrieve a list of attendees
            //curl https://localhost:44300/api/attendeedata/listattendees


            string url = "attendeedata/listattendees";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<AttendeeDto> attendees = response.Content.ReadAsAsync<IEnumerable<AttendeeDto>>().Result;

            //Debug.WriteLine("Number of attendees received ");
            //Debug.WriteLine(attendees.Count());

            return View(attendees);
        }

        // GET: Attendee/Details/5
        public ActionResult View(int id)
        {
            DetailsAttendee ViewModel = new DetailsAttendee();

            //objective: communicate with my attendee data api to retrieve one attendee
            //curl https://localhost:44300/api/attendeedata/findattendee/id


            string url = "attendeedata/findattendee/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            AttendeeDto selectedattendee = response.Content.ReadAsAsync<AttendeeDto>().Result;

            //Debug.WriteLine("Attendee received: ");
            //Debug.WriteLine(selectedattendee.FirstName);
            //Debug.WriteLine(selectedattendee.LastName);

            ViewModel.SelectedAttendee = selectedattendee;

            //show all events that the selected attendee signed up for

            url = "receptiondata/listreceptionsforattendee/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<ReceptionDto> RegEvents = response.Content.ReadAsAsync<IEnumerable<ReceptionDto>>().Result;
            //JsonConvert.DeserializeObject<List<AttendeeDto>>();

            ViewModel.RegEvents = RegEvents;

            return View(ViewModel);
        }




        public ActionResult Errors()
        {
            return View();
        }

        // GET: Attendee/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Attendee/Create
        [HttpPost]
        public ActionResult Create(Attendee attendee)
        {
            Debug.WriteLine("The json payload is ");
            //Debug.WriteLine(attendee.FirstName + " " + attendee.LastName);
            //objective add a new attendee to the API
            string url = "attendeedata/addattendee";


            string jsonpayload = jss.Serialize(attendee);

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

        // GET: Attendee/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "attendeedata/findattendee/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AttendeeDto selectedattendee = response.Content.ReadAsAsync<AttendeeDto>().Result;
            return View(selectedattendee);
        }

        // POST: Attendee/Update/5
        [HttpPost]
        public ActionResult Update(int id, Attendee attendee)
        {
            string url = "attendeedata/updateattendee/" + id;
            string jsonpayload = jss.Serialize(attendee);
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

        // GET: Attendee/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "attendeedata/findattendee/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AttendeeDto selectedattendee = response.Content.ReadAsAsync<AttendeeDto>().Result;
            return View(selectedattendee);
        }

        // POST: Attendee/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Attendee attendee)
        {
            string url = "attendeedata/deleteattendee/" + id;
            string jsonpayload = jss.Serialize(attendee);
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
