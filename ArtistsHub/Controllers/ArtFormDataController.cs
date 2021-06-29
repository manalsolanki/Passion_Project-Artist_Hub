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
                DisciplineId = element.DisciplineID,
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
                DisciplineId = artForm.DisciplineID,
                DisciplineName = artForm.Discipline.DisciplineName
            };
            if (artForm == null)
            {
                return NotFound();
            }

            return Ok(artFormDto);
        }

        /// <summary>
        /// Gathers information about all artForms related to a particular discipline ID
        /// </summary>
        /// <returns>
        /// CONTENT: all artforms in the database, including their associated discipline matched with a particular discipline ID
        /// </returns>
        /// <param name="id">Discipline ID.</param>
        /// <example>
        /// GET: api/ArtFormData/ListArtFormsForDisciplines/3
        /// </example>
        [HttpGet]
        [ResponseType(typeof(ArtFormDto))]
        public IHttpActionResult ListArtFormsForDisciplines(int id)
        {
            List<ArtForm> artForms = db.ArtForms.Where(a=>a.DisciplineID == id).ToList();
            List<ArtFormDto> artFormDtos = new List<ArtFormDto>();
            artForms.ForEach(artForm => artFormDtos.Add(new ArtFormDto()
            {
                ArtFormID = artForm.ArtFormID,
                ArtFormName = artForm.ArtFormName,
                DisciplineId = artForm.DisciplineID,
                DisciplineName = artForm.Discipline.DisciplineName
            }));
            
            return Ok(artFormDtos);
        }


        /// <summary>
        /// Gathers information about all artForms related to a particular Artist
        /// </summary>
        /// <returns>
        /// CONTENT: all artforms in the database, including their associated artist matched with a particular Artist ID
        /// </returns>
        /// <param name="id">Artist ID.</param>
        /// <example>
        /// GET: api/ArtFormData/ListArtFormsForArtist/3
        /// </example>
        [HttpGet]
        [ResponseType(typeof(ArtFormDto))]
        public IHttpActionResult ListArtFormsForArtist(int id)
        {
            List<ArtForm> artForms = db.ArtForms.Where(
                af => af.Artists.Any(
                    at => at.ArtistID == id 
                    )).ToList();
            List<ArtFormDto> artFormDtos = new List<ArtFormDto>();
            artForms.ForEach(artForm => artFormDtos.Add(new ArtFormDto()
            {
                ArtFormID = artForm.ArtFormID,
                ArtFormName = artForm.ArtFormName,
                DisciplineId = artForm.DisciplineID,
                DisciplineName = artForm.Discipline.DisciplineName
            }));

            return Ok(artFormDtos);
        }

        /// <summary>
        /// Gathers information about all artForms which is not interested by a particular Artist
        /// </summary>
        /// <returns>
        /// CONTENT: all artforms in the database, which an artist is not interested in.
        /// </returns>
        /// <param name="id">Artist primary key</param>
        /// <example>
        /// GET: api/ArtFormData/ListArtFormsNotInterestedByArtist/2
        /// </example>
        [HttpGet]
        [ResponseType(typeof(ArtFormDto))]
        public IHttpActionResult ListArtFormsNotInterestedByArtist(int id)
        {
            List<ArtForm> artForms = db.ArtForms.Where(
                af => !af.Artists.Any(
                    at => at.ArtistID == id
                    )).ToList();
            List<ArtFormDto> artFormDtos = new List<ArtFormDto>();
            artForms.ForEach(artForm => artFormDtos.Add(new ArtFormDto()
            {
                ArtFormID = artForm.ArtFormID,
                ArtFormName = artForm.ArtFormName,
                DisciplineId = artForm.DisciplineID,
                DisciplineName = artForm.Discipline.DisciplineName
            }));

            return Ok(artFormDtos);
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