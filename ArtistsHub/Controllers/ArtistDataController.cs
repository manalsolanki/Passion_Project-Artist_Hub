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
using ArtistsHub.Models;
using System.Diagnostics;
using System.Web;
using System.IO;

namespace ArtistsHub.Controllers
{
    public class ArtistDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// List all the artit from the db
        /// </summary>
        /// <returns>A list of artist</returns>
        // GET: api/ArtistData/ListArtists
        [HttpGet]
        public IEnumerable<ArtistDto> ListArtists()
        {

            List<Artist> artists = db.Artists.ToList();
            List<ArtistDto> artistDtos = new List<ArtistDto>();
            artists.ForEach (element => artistDtos.Add(new ArtistDto()
            {
                ArtistID = element.ArtistID,
                ArtistFirstName = element.ArtistFirstName,
                ArtistLastName = element.ArtistLastName,
                ArtistOccupation = element.ArtistOccupation,
                ArtistDescription = element.ArtistDescription,
                ArtistEmail = element.ArtistEmail,
                ArtistPhoneNumber = element.ArtistPhoneNumber,
                ArtistHasProfilePic = element.ArtistHasProfilePic,
                ProfilePicExtension = element.ProfilePicExtension
            })) ;
                
            return artistDtos;
        }
        /// <summary>
        /// Finds the artist by artist id
        /// </summary>
        /// <param name="id">Artist ID - Primary key</param>
        /// <returns>A artist.</returns>
        // GET: api/ArtistData/FindArtist/5
        [ResponseType(typeof(Artist))]
        [HttpGet]
        public IHttpActionResult FindArtist(int id)
        {
            Artist artist = db.Artists.Find(id);
            ArtistDto artistDto = new ArtistDto()
            {
                ArtistID= artist.ArtistID,
                ArtistFirstName = artist.ArtistFirstName,
                ArtistLastName = artist.ArtistLastName,
                ArtistOccupation = artist.ArtistOccupation,
                ArtistEmail = artist.ArtistEmail,
                ArtistPhoneNumber= artist.ArtistPhoneNumber,
                ArtistDescription= artist.ArtistDescription,
                ArtistHasProfilePic = artist.ArtistHasProfilePic,
                ProfilePicExtension = artist.ProfilePicExtension
            };
            if (artist == null)
            {
                return NotFound();
            }

            return Ok(artistDto);
        }
       /// <summary>
       /// List the artists for a particular art form.
       /// </summary>
       /// <param name="id">Art form id</param>
       /// <returns>A list of artist./returns>
        // Artform id will be included as part of url.
        // GET: api/ArtistData/ListArtistForArtForm/2
        
        [HttpGet]
        [ResponseType(typeof(ArtistDto))]
        public IHttpActionResult ListArtistForArtForm(int id)
        {
            List<Artist> artists = db.Artists.Where(
                at => at.ArtForms.Any(
                    af => af.ArtFormID == id
                    )).ToList();
            List<ArtistDto> artistDtos = new List<ArtistDto>();
            artists.ForEach(artist => artistDtos.Add(new ArtistDto()
            {
                ArtistID = artist.ArtistID,
                ArtistFirstName = artist.ArtistFirstName,
                ArtistLastName = artist.ArtistLastName,
                ArtistDescription = artist.ArtistDescription,
                ArtistEmail = artist.ArtistEmail,
                ArtistOccupation= artist.ArtistOccupation,
                ArtistPhoneNumber = artist.ArtistPhoneNumber
            }));

            return Ok(artistDtos);
        }

        /// <summary>
        /// Associate a particular artform to a artist.
        /// </summary>
        /// <param name="artistId">Artist Id Primary key</param>
        /// <param name="ArtFormId">Artform id primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// Post: api/ArtistData/AssociateArtFormsWithArtist/1/3;
        ///curl -d "" http://localhost:49268/api/ArtistData/AssociateArtFormsWithArtist/1/3
        /// </example>
        [HttpPost]
        [Route("api/ArtistData/AssociateArtFormsWithArtist/{artistId}/{ArtFormId}")]
        public IHttpActionResult AssociateArtFormsWithArtist(int artistId , int ArtFormId)
        {
            Debug.WriteLine(artistId);
            Artist SelectedArtist = db.Artists.Include(af => af.ArtForms).Where(af => af.ArtistID == artistId).FirstOrDefault();
            ArtForm SelectedArtForm = db.ArtForms.Find(ArtFormId);
            Debug.WriteLine("IN Artist Data Cntrl.");
            Debug.WriteLine(SelectedArtist);
            Debug.WriteLine(SelectedArtForm);
            if (SelectedArtist == null || SelectedArtForm == null)
            {
                return NotFound();
            }

            SelectedArtist.ArtForms.Add(SelectedArtForm);
            db.SaveChanges();

            return Ok();
        }
        /// <summary>
        /// UnAssociates a particular artform to a artist.
        /// </summary>
        /// <param name="artistId">Artist Id Primary key</param>
        /// <param name="ArtFormId">Artform id primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// Post: api/ArtistData/UnAssociateArtFormsWithArtist/1/3;
        ///curl -d "" http://localhost:49268/api/ArtistData/AssociateArtFormsWithArtist/1/3
        /// </example>
        [HttpPost]
        [Route("api/ArtistData/UnAssociateArtFormsWithArtist/{artistId}/{ArtFormId}")]
        public IHttpActionResult UnAssociateArtFormsWithArtist(int artistId, int ArtFormId)
        {
            Debug.WriteLine(artistId);
            Artist SelectedArtist = db.Artists.Include(af => af.ArtForms).Where(af => af.ArtistID == artistId).FirstOrDefault();
            ArtForm SelectedArtForm = db.ArtForms.Find(ArtFormId);
            Debug.WriteLine(SelectedArtist);
            Debug.WriteLine(SelectedArtForm);
            if (SelectedArtist == null || SelectedArtForm == null)
            {
                return NotFound();
            }

            SelectedArtist.ArtForms.Remove(SelectedArtForm);
            db.SaveChanges();

            return Ok();
        }


        // POST: api/ArtistData/UpdateArtist/3
        //curl -d @artist.json -H "Content-type:application/json" http://localhost:49268/api/ArtistData/UpdateArtist/3
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateArtist(int id, Artist artist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != artist.ArtistID)
            {
                return BadRequest();
            }

            db.Entry(artist).State = EntityState.Modified;


            // Picture update is handled by another method
            db.Entry(artist).Property(a => a.ArtistHasProfilePic).IsModified = false;
            db.Entry(artist).Property(a => a.ProfilePicExtension).IsModified = false;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArtistExists(id))
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
        /// Receives artist Profile picture data, uploads it to the webserver and updates the artist HasPic option
        /// </summary>
        /// <param name="id">Artist Id</param>
        /// <returns>status code 200 if successful.</returns>
        /// <example>
        /// curl -F artistprofilepic=@file.jpg "https://localhost:xx/api/Artistdata/uploadArtistpic/2"
        /// POST: api/ArtistData/UploadArtistPic/3
        /// HEADER: enctype=multipart/form-data
        /// FORM-DATA: image
        /// </example>

        [HttpPost]
        public IHttpActionResult UploadArtistPic(int id)
        {

            bool haspic = false;
            string picextension;
            if (Request.Content.IsMimeMultipartContent())
            {
                Debug.WriteLine("Received multipart form data.");

                int numfiles = HttpContext.Current.Request.Files.Count;
                Debug.WriteLine("Files Received: " + numfiles);

                //Check if a file is posted
                if (numfiles == 1 && HttpContext.Current.Request.Files[0] != null)
                {
                    var artistProfilePic = HttpContext.Current.Request.Files[0];
                    //Check if the file is empty
                    if (artistProfilePic.ContentLength > 0)
                    {
                        //establish valid file types (can be changed to other file extensions if desired!)
                        var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                        var extension = Path.GetExtension(artistProfilePic.FileName).Substring(1);
                        //Check the extension of the file
                        if (valtypes.Contains(extension))
                        {
                            try
                            {
                                //file name is the id of the image
                                string fn = id + "." + extension;

                                //get a direct file path to ~/Content/animals/{id}.{extension}
                                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Images/Artists/"), fn);

                                //save the file
                                artistProfilePic.SaveAs(path);

                                //if these are all successful then we can set these fields
                                haspic = true;
                                picextension = extension;

                                //Update the animal haspic and picextension fields in the database
                                Artist Selectedartist = db.Artists.Find(id);
                                Selectedartist.ArtistHasProfilePic = haspic;
                                Selectedartist.ProfilePicExtension = extension;
                                db.Entry(Selectedartist).State = EntityState.Modified;

                                db.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("Artist Image was not saved successfully.");
                                Debug.WriteLine("Exception:" + ex);
                                return BadRequest();
                            }
                        }
                    }

                }

                return Ok();
            }
            else
            {
                //not multipart form data
                return BadRequest();

            }

        }



        // POST: api/ArtistData/AddArtist
        //curl -d @artist.json -H "Content-type:application/json" http://localhost:49268/api/ArtistData/AddArtist
        [ResponseType(typeof(Artist))]
        [HttpPost]
        public IHttpActionResult AddArtist(Artist artist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Artists.Add(artist);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = artist.ArtistID }, artist);
        }



        // POST: api/ArtistData/DeleteArtist/3
        // curl - d "" http://localhost:49268/api/ArtistData/DeleteArtist/3
        [ResponseType(typeof(Artist))]
        [HttpPost]
        public IHttpActionResult DeleteArtist(int id)
        {
            Artist artist = db.Artists.Find(id);
            if (artist == null)
            {
                return NotFound();
            }
            if(artist.ArtistHasProfilePic && artist.ProfilePicExtension != "")
            {
                string path = HttpContext.Current.Server.MapPath("~/Content/Images/Artists/" + id + "." + artist.ProfilePicExtension);
                if (System.IO.File.Exists(path))
                {
                    Debug.WriteLine("File exists... preparing to delete!");
                    System.IO.File.Delete(path);
                }
            }
            db.Artists.Remove(artist);
            db.SaveChanges();

            return Ok(artist);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ArtistExists(int id)
        {
            return db.Artists.Count(e => e.ArtistID == id) > 0;
        }
    }
}