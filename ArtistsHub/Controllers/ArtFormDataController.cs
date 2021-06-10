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
    public class ArtFormDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ArtFormData/ListArtForms
        [HttpGet]
        public IEnumerable<ArtFormDto> ListArtForms()
        {
            List<ArtForm> artForms = db.ArtForms.ToList();
            List<ArtFormDto> artFormDtos = new List<ArtFormDto>();
            artForms.ForEach(element => artFormDtos.Add(new ArtFormDto()
            {
                ArtFormID = element.ArtFormID,
                ArtFormName = element.ArtFormName,
                DisciplineName = element.Discipline.DisciplineName
            }));
            return artFormDtos;
        }

        // GET: api/ArtFormData/FindArtForm/1
        [ResponseType(typeof(ArtForm))]
        [HttpGet]
        public IHttpActionResult FindArtForm(int id)
        {
            ArtForm artForm = db.ArtForms.Find(id);
            ArtFormDto artFormDto = new ArtFormDto()
            {
                ArtFormID = artForm.ArtFormID,
                ArtFormName = artForm.ArtFormName,
                DisciplineName = artForm.Discipline.DisciplineName
            };
            if (artForm == null)
            {
                return NotFound();
            }

            return Ok(artFormDto);
        }

        // POST: api/ArtFormData/UpdateArtForm/5
        //curl -d @artform.json -H "Content-type:application/json" http://localhost:49268/api/ArtFormData/UpdateArtForm/6
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateArtForm(int id, ArtForm artForm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != artForm.ArtFormID)
            {
                return BadRequest();
            }

            db.Entry(artForm).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArtFormExists(id))
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

        // POST: api/ArtFormData/AddArtForm
        //curl -d @artform.json -H "Content-type:application/json" http://localhost:49268/api/ArtFormData/AddArtForm
        [ResponseType(typeof(ArtForm))]
        [HttpPost]
        public IHttpActionResult AddArtForm(ArtForm artForm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ArtForms.Add(artForm);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = artForm.ArtFormID }, artForm);
        }

        // POST: api/ArtFormData/DeleteArtForm/1
        // curl -d "" http://localhost:49268/api/DeleteArtForm/1
        [ResponseType(typeof(ArtForm))]
        [HttpPost]
        public IHttpActionResult DeleteArtForm(int id)
        {
            ArtForm artForm = db.ArtForms.Find(id);
            if (artForm == null)
            {
                return NotFound();
            }

            db.ArtForms.Remove(artForm);
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

        private bool ArtFormExists(int id)
        {
            return db.ArtForms.Count(e => e.ArtFormID == id) > 0;
        }
    }
}