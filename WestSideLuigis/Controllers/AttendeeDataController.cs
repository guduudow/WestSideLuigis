using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WestSideLuigis.Models;

namespace WestSideLuigis.Controllers
{
    public class AttendeeDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all attendees in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all attendees in the database
        /// </returns>
        /// <example>
        /// GET: api/AttendeeData/ListAttendee
        /// </example>


        [HttpGet]
        public IEnumerable<AttendeeDto> ListAttendees()
        {
            List<Attendee> Attendees = db.Attendees.ToList();
            List<AttendeeDto> AttendeeDtos = new List<AttendeeDto>();

            Attendees.ForEach(a => AttendeeDtos.Add(new AttendeeDto()
            {
                AttendeeID = a.AttendeeID,
                FirstName = a.FirstName,
                LastName = a.LastName,
                Email = a.Email,
                PhoneNumber = a.PhoneNumber
            }));

            return AttendeeDtos;
        }


        /// <summary>
        /// Returns all attendees in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An attendee in the system matching up to the attendee ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the attendee</param>
        /// <example>
        /// GET: api/AttendeeData/FindAttendee/5
        /// </example>


        [ResponseType(typeof(Attendee))]
        [HttpGet]
        public IHttpActionResult FindAttendee(int id)
        {
            Attendee Attendee = db.Attendees.Find(id);
            AttendeeDto AttendeeDto = new AttendeeDto()
            {
                AttendeeID = Attendee.AttendeeID,
                FirstName = Attendee.FirstName,
                LastName = Attendee.LastName,
                Email = Attendee.Email,
                PhoneNumber = Attendee.PhoneNumber
            };
            if (Attendee == null)
            {
                return NotFound();
            }

            return Ok(AttendeeDto);
        }

        /// <summary>
        /// Updates a particular attendee in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Attendee ID primary key</param>
        /// <param name="attendee">JSON FORM DATA of an attendee</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/AttendeeData/UpdateAttendee/5
        /// FORM DATA: Attendee JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateAttendee(int id, Attendee attendee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != attendee.AttendeeID)
            {
                return BadRequest();
            }

            db.Entry(attendee).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AttendeeExists(id))
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
        /// Adds an attendee to the system
        /// </summary>
        /// <param name="attendee">JSON FORM DATA of an attendee</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Attendee ID, Attendee Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/AttendeeData/AddAttendee
        /// FORM DATA: Attendee JSON Object
        /// </example>
        [ResponseType(typeof(Attendee))]
        [HttpPost]
        public IHttpActionResult AddAttendee(Attendee attendee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Attendees.Add(attendee);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = attendee.AttendeeID }, attendee);
        }

        /// <summary>
        /// Deletes a attendee from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the attendee</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/AttendeeData/DeleteAttendee/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Attendee))]
        [HttpPost]
        public IHttpActionResult DeleteAttendee(int id)
        {
            Attendee attendee = db.Attendees.Find(id);
            if (attendee == null)
            {
                return NotFound();
            }

            db.Attendees.Remove(attendee);
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

        private bool AttendeeExists(int id)
        {
            return db.Attendees.Count(e => e.AttendeeID == id) > 0;
        }

        ///<summary>
        ///Gathers information about attendees related to a specific event
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: Reception has the names of the attendees going to the event
        /// </returns>
        /// <param name="id">Reception ID</param>
        /// <example>
        /// GET: api/AttendeeData/ListAttendeesForEvent/2
        /// </example>
        /// 
        [HttpGet]
        public IHttpActionResult ListAttendeesForReception(int id)
        {
            //all attendees that have events which match with our id
            List<Attendee> Attendees = db.Attendees.Where(
               a => a.Receptions.Any(
                r => r.ReceptionID == id
                )).ToList();
            List<AttendeeDto> AttendeeDtos = new List<AttendeeDto>();

            Attendees.ForEach(a => AttendeeDtos.Add(new AttendeeDto()
            {
                AttendeeID = a.AttendeeID,
                FirstName = a.FirstName,
                LastName = a.LastName,
                Email = a.Email,
                PhoneNumber = a.PhoneNumber
            }));

            return Ok(AttendeeDtos);
        }

        ///<summary>
        ///Gathers information about attendees related to a specific event
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: dropdown showing all the attendees that have not signed up for particular event
        /// </returns>
        /// <param name="id">Reception ID</param>
        /// <example>
        /// GET: api/AttendeeData/ListAttendeesNotSignedUpForReception/2
        /// </example>
        /// 
        [HttpGet]
        public IHttpActionResult ListAttendeesNotSignedUpForReception(int id)
        {
            //all attendees that have events which match with our id
            List<Attendee> Attendees = db.Attendees.Where(
               a => !a.Receptions.Any(
                r => r.ReceptionID == id
                )).ToList();
            List<AttendeeDto> AttendeeDtos = new List<AttendeeDto>();

            Attendees.ForEach(a => AttendeeDtos.Add(new AttendeeDto()
            {
                AttendeeID = a.AttendeeID,
                FirstName = a.FirstName,
                LastName = a.LastName,
                Email = a.Email,
                PhoneNumber = a.PhoneNumber
            }));

            return Ok(AttendeeDtos);
        }

        /// <summary>
        /// Adds a particular attendee with a particular reception
        /// </summary>
        /// <param name="attendeeid">The attendee ID primary key</param>
        /// <param name="receptionid">The reception ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/AttendeeData/AddAttendeeToReception/9/1
        /// </example>
        [HttpPost]
        [Route("api/attendeedata/AddAttendeeToReception/{receptionid}/{attendeeid}")]
        public IHttpActionResult AddAttendeeToReception(int receptionid, int attendeeid)
        {
            Attendee SelectedAttendee = db.Attendees.Include(a => a.Receptions).Where(a => a.AttendeeID == attendeeid).FirstOrDefault();
            Reception SelectedReception = db.Receptions.Find(receptionid);

            if (SelectedAttendee == null || SelectedReception == null)
            {
                return NotFound();
            }

            Debug.WriteLine("input attende id is: " + attendeeid);
            Debug.WriteLine("selected attendee name is: " + SelectedAttendee.FirstName + " " + SelectedAttendee.LastName);
            Debug.WriteLine("input reception id is: " + receptionid);
            Debug.WriteLine("selected reception name is: " + SelectedReception.ReceptionName);


            SelectedAttendee.Receptions.Add(SelectedReception);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Removes a particular attendee with a particular reception
        /// </summary>
        /// <param name="attendeeid">The attendee ID primary key</param>
        /// <param name="receptionid">The reception ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/AttendeeData/RemoveAttendeeFromReception/9/1
        /// </example>
        [HttpPost]
        [Route("api/attendeedata/RemoveAttendeeFromReception/{receptionid}/{attendeeid}")]
        public IHttpActionResult RemoveAttendeeFromReception(int receptionid, int attendeeid)
        {
            Attendee SelectedAttendee = db.Attendees.Include(a => a.Receptions).Where(a => a.AttendeeID == attendeeid).FirstOrDefault();
            Reception SelectedReception = db.Receptions.Find(receptionid);

            if (SelectedAttendee == null || SelectedReception == null)
            {
                return NotFound();
            }

            Debug.WriteLine("input attende id is: " + attendeeid);
            Debug.WriteLine("selected attendee name is: " + SelectedAttendee.FirstName + " " + SelectedAttendee.LastName);
            Debug.WriteLine("input reception id is: " + receptionid);
            Debug.WriteLine("selected reception name is: " + SelectedReception.ReceptionName);


            SelectedAttendee.Receptions.Remove(SelectedReception);
            db.SaveChanges();

            return Ok();
        }


    }
}