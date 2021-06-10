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

namespace ArtistsHub.Controllers
{
    public class ArtistDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ArtistData/ListArtists
        [HttpGet]
        public IQueryable<Artist> ListArtist()
        {
            return db.Artists;
        }

        // GET: api/ArtistData/Updatertist/5
        [ResponseType(typeof(Artist))]
        [HttpGet]
        public IHttpActionResult UpdateArtist(int id)
        {
            Artist artist = db.Artists.Find(id);
            if (artist == null)
            {
                return NotFound();
            }

            return Ok(artist);
        }

        // POST: api/ArtistData/UpdateArtist/5
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

        // POST: api/ArtistData/AddArtist
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

        // POST: api/ArtistData/DeleteArtist/5
        [ResponseType(typeof(Artist))]
        [HttpPost]
        public IHttpActionResult DeleteArtist(int id)
        {
            Artist artist = db.Artists.Find(id);
            if (artist == null)
            {
                return NotFound();
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