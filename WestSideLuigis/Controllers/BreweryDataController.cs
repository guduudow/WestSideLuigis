using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using WestSideLuigis.Models;

namespace TorontoBeerDirectory.Controllers
{
    public class BreweryDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all breweries in the database
        /// </summary>
        /// <returns>
        /// if the connection is successful it returns all breweries in the database 
        /// </returns>
        /// <example>
        /// GET: api/BreweryData/ListBreweries
        /// </example>

        // GET: api/BreweryData/ListBreweries
        [HttpGet]
        public IQueryable<Brewery> ListBreweries()
        {
            return db.Breweries;
        }

        /// <summary>
        /// Returns specific brewery in the system with a matching id
        /// </summary>
        /// <param name="id">The primary key of the brewery</param>
        /// <returns>
        /// A brewery matching the specific BreweryID primary key if connection is successful
        /// </returns>
        /// <example>
        /// GET: api/BreweryData/FindBrewery/1
        /// </example>

        // GET: api/BreweryData/FindBrewery/1
        [ResponseType(typeof(Brewery))]
        [HttpGet]
        public IHttpActionResult FindBrewery(int id)
        {
            Brewery Brewery = db.Breweries.Find(id);
            //BreweryDto Brewerydto = new BreweryDto()
            //{
            //    BreweryID = Brewery.BreweryID,
            //    BreweryName = Brewery.BreweryName,
            //    BreweryLocation = Brewery.BreweryLocation
            //};
            if (Brewery == null)
            {
                return NotFound();
            }

            return Ok(Brewery);
        }

        /// <summary>
        /// Updates a specific brewery in the database with a POST request
        /// </summary>
        /// <param name="id">BreweryID primary key</param>
        /// <param name="brewery">JSON data of a brewery</param>
        /// <returns>
        /// On successful connection database is updated
        /// </returns>
        /// <example>
        /// POST: api/BreweryData/UpdateBrewery/1
        /// DATA: Brewery JSON object
        /// </example>

        // POST: api/BreweryData/UpdateBrewery/1
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateBrewery(int id, Brewery brewery)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != brewery.BreweryID)
            {
                return BadRequest();
            }

            db.Entry(brewery).State = EntityState.Modified;
            //Pic update is handled by method UploadBreweryPic()
            db.Entry(brewery).Property(b => b.BreweryHasPic).IsModified = false;
            db.Entry(brewery).Property(b => b.PicExtension).IsModified = false;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BreweryExists(id))
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
        /// Receives brewery picture data, uploads it to the webserver and updates the breweryhaspic
        /// </summary>
        /// <param name="id">the brewery id</param>
        /// <returns>status code 200 if successful.</returns>
        /// <example>
        /// POST: api/BreweryData/UpdatebreweryPic/1
        /// HEADER: enctype=multipart/form-data
        /// FORM-DATA: image
        /// </example>
        /// Below is stackoverflow help (use when needed)
        /// https://stackoverflow.com/questions/28369529/how-to-set-up-a-web-api-controller-for-multipart-form-data

        [HttpPost]
        public IHttpActionResult UploadBreweryPic(int id)
        {
            bool haspic = false;
            string picextension;
            if (Request.Content.IsMimeMultipartContent())
            {
                Debug.WriteLine("Recieved multiport form data.");

                int numfiles = HttpContext.Current.Request.Files.Count;
                Debug.WriteLine("Files Received: " + numfiles);

                //Check if a file is posted
                if (numfiles == 1 && HttpContext.Current.Request.Files[0] != null)
                {
                    var breweryPic = HttpContext.Current.Request.Files[0];
                    //Check if file is empty (checks the file size)
                    if (breweryPic.ContentLength > 0)
                    {
                        //establish valid file types (can be changed to other file extensions if desired)
                        var valtypes = new[] { "jpeg", "jpg", "png" };
                        var extension = Path.GetExtension(breweryPic.FileName).Substring(1);
                        //CHeck the extension of the file
                        if (valtypes.Contains(extension))
                        {
                            try
                            {
                                //file name is the id of the image
                                string fn = id + "." + extension;

                                //get a direct file path tp ~/Content/Images/Breweries/{id}.{extension}
                                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Images/Breweries/"), fn);

                                //save the file
                                breweryPic.SaveAs(path);

                                //if everything is successful then we can set the following fields
                                haspic = true;
                                picextension = extension;

                                //Update the brewery haspic and picextension fields in the database
                                Brewery SelectedBrewery = db.Breweries.Find(id);
                                SelectedBrewery.BreweryHasPic = haspic;
                                SelectedBrewery.PicExtension = extension;
                                db.Entry(SelectedBrewery).State = EntityState.Modified;

                                db.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("Brewery Image was not save successfully...");
                                Debug.WriteLine("Exception: " + ex);
                                return BadRequest();
                            }
                        }
                    }
                }
                return Ok();
            }
            else
            {
                //not multiport form data
                return BadRequest();
            }
        }

        /// <summary>
        /// Adds a brewery to the database
        /// </summary>
        /// <param name="brewery">JSON data of a brewery</param>
        /// <returns>
        /// adds BreweryID and the BreweryData with it
        /// </returns>
        /// <example>
        /// POST: api/BreweryData/AddBrewery
        /// Data: Brewery JSON DATA
        /// </example>

        // POST: api/BreweryData/AddBrewery
        [ResponseType(typeof(Brewery))]
        [HttpPost]
        public IHttpActionResult AddBrewery(Brewery brewery)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Breweries.Add(brewery);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = brewery.BreweryID }, brewery);
        }

        /// <summary>
        /// Deletes a brewery from the database by its ID
        /// </summary>
        /// <param name="id">Primary key for Brewery</param>
        /// <returns>
        /// entry deleted from the database
        /// </returns>
        /// <example>
        /// POST: api/BreweryData/DeleteBrewery/1
        /// DATA: there will be none because its deleted
        /// </example>

        // DELETE: api/BreweryData/DeleteBrewery/1
        [ResponseType(typeof(Brewery))]
        [HttpPost]
        public IHttpActionResult DeleteBrewery(int id)
        {
            Brewery brewery = db.Breweries.Find(id);
            if (brewery == null)
            {
                return NotFound();
            }

            db.Breweries.Remove(brewery);
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

        private bool BreweryExists(int id)
        {
            return db.Breweries.Count(e => e.BreweryID == id) > 0;
        }
    }
}