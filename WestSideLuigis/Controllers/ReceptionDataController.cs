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
    public class ReceptionDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all receptions in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all receptions in the database
        /// </returns>
        /// <example>
        /// GET: api/ReceptionData/ListReceptions
        /// </example>
        // 
        [HttpGet]
        public IEnumerable<ReceptionDto> ListReceptions()
        {
            List<Reception> Receptions = db.Receptions.ToList();
            List<ReceptionDto> ReceptionDtos = new List<ReceptionDto>();

            Receptions.ForEach(e => ReceptionDtos.Add(new ReceptionDto()
            {
                ReceptionID = e.ReceptionID,
                ReceptionName = e.ReceptionName,
                ReceptionLocation = e.ReceptionLocation,
                ReceptionDate = e.ReceptionDate,
                StartTime = e.StartTime,
                EndTime = e.EndTime,
                ReceptionPrice = e.ReceptionPrice,
                ReceptionDescription = e.ReceptionDescription
            }));

            return ReceptionDtos;
        }

        /// <summary>
        /// Returns all receptions in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: A reception in the system matching up to the Reception ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Reception</param>
        /// <example>
        /// GET: api/ReceptionData/FindReception/5
        /// </example>

        [ResponseType(typeof(Reception))]
        [HttpGet]
        public IHttpActionResult FindReception(int id)
        {
            Reception Reception = db.Receptions.Find(id);
            ReceptionDto ReceptionDto = new ReceptionDto()
            {
                ReceptionID = Reception.ReceptionID,
                ReceptionName = Reception.ReceptionName,
                ReceptionLocation = Reception.ReceptionLocation,
                ReceptionDate = Reception.ReceptionDate,
                StartTime = Reception.StartTime,
                EndTime = Reception.EndTime,
                ReceptionPrice = Reception.ReceptionPrice,
                ReceptionDescription = Reception.ReceptionDescription
            };
            if (Reception == null)
            {
                return NotFound();
            }

            return Ok(ReceptionDto);
        }


        /// <summary>
        /// Updates a particular reception in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the reception ID primary key</param>
        /// <param name="reception">JSON FORM DATA of a reception</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/ReceptionData/UpdateReception/5
        /// FORM DATA: reception JSON Object
        /// </example>


        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public IHttpActionResult UpdateReception(int id, Reception Reception)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Reception.ReceptionID)
            {
                return BadRequest();
            }

            db.Entry(Reception).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReceptionExists(id))
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
        /// Adds a reception to the system
        /// </summary>
        /// <param name="Reception">JSON FORM DATA of an reception</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Reception ID, Reception Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/ReceptionData/AddReception
        /// FORM DATA: Reception JSON Object
        /// </example>


        [ResponseType(typeof(Reception))]
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public IHttpActionResult AddReception(Reception Reception)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Receptions.Add(Reception);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Reception.ReceptionID }, Reception);
        }


        /// <summary>
        /// Deletes a reception from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the reception</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/ReceptionData/DeleteReception/5
        /// FORM DATA: (empty)
        /// </example>


        [ResponseType(typeof(Reception))]
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public IHttpActionResult DeleteReception(int id)
        {
            Reception Reception = db.Receptions.Find(id);
            if (Reception == null)
            {
                return NotFound();
            }

            db.Receptions.Remove(Reception);
            db.SaveChanges();

            return Ok(Reception);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReceptionExists(int id)
        {
            return db.Receptions.Count(e => e.ReceptionID == id) > 0;
        }

        ///<summary>
        ///Gathers information about events related to a specific attendee
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: Attendee has the names of the receptions they signed up for
        /// </returns>
        /// <param name="id">Attendee ID</param>
        /// <example>
        /// GET: api/AttendeeData/ListReceptionsForAttendee/2
        /// </example>
        /// 
        [HttpGet]
        [ResponseType(typeof(ReceptionDto))]
        public IHttpActionResult ListReceptionsForAttendee(int id)
        {
            //all receptions that have attendees which match with our id
            List<Reception> Receptions = db.Receptions.Where(
               r => r.Attendees.Any(
                a => a.AttendeeID == id
                )).ToList();
            List<ReceptionDto> ReceptionDtos = new List<ReceptionDto>();

            Receptions.ForEach(r => ReceptionDtos.Add(new ReceptionDto()
            {
                ReceptionID = r.ReceptionID,
                ReceptionName = r.ReceptionName,
                ReceptionLocation = r.ReceptionLocation,
                StartTime = r.StartTime,
                EndTime = r.EndTime,
                ReceptionPrice = r.ReceptionPrice,
                ReceptionDescription = r.ReceptionDescription
            }));

            return Ok(ReceptionDtos);
        }
    }
}