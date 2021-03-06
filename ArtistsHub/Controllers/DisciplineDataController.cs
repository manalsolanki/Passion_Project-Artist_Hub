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

namespace ArtistsHub.Controllers
{
    public class DisciplineDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// To list all the disciplines
        /// </summary>
        /// <returns></returns>
        // GET: api/DisciplineData/ListDisciplines
        [HttpGet]
        public IEnumerable<DisciplineDto> ListDisciplines()
        {
            List<Discipline> disciplines = db.Disciplines.ToList();
            List<DisciplineDto> disciplineDtos = new List<DisciplineDto>();
            disciplines.ForEach(element => disciplineDtos.Add(new DisciplineDto()
            {
                DisciplineID= element.DisciplineID,
                DisciplineName=element.DisciplineName,
                DisciplineDescription = element.DisciplineDescription
            }));
            return disciplineDtos;
        }

        /// <summary>
        /// Find the dicipline by Id.
        /// </summary>
        /// <param name="id">Discipline Id</param>
        /// <returns>A specific discipline</returns>
        // GET: api/DisciplineData/FindDiscipline/5
        [ResponseType(typeof(Discipline))]
        [HttpGet]
        public IHttpActionResult FindDiscipline(int id)
        {
            Discipline discipline = db.Disciplines.Find(id);
            DisciplineDto disciplineDto = new DisciplineDto()
            {
                DisciplineID = discipline.DisciplineID,
                DisciplineName = discipline.DisciplineName,
                DisciplineDescription = discipline.DisciplineDescription
            };

            if (discipline == null)
            {
                return NotFound();
            }

            return Ok(disciplineDto);
        }

        // POST: api/DisciplineData/UpdateDiscipline/4
        // curl -d @discipline.json -H "Content-type:application/json" http://localhost:49268/api/DisciplineData/Updateiscipline/4
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateDiscipline(int id, Discipline discipline)
        {
            Debug.WriteLine("In Data COntroller");
            Debug.WriteLine(discipline);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != discipline.DisciplineID)
            {
                return BadRequest();
            }

            db.Entry(discipline).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DisciplineExists(id))
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

        // POST: api/DisciplineData/AddDiscipline
        // curl -d @discipline.json -H "Content-type:application/json" http://localhost:49268/api/DisciplineData/AddDiscipline
        [ResponseType(typeof(Discipline))]
        [HttpPost]
        public IHttpActionResult AddDiscipline(Discipline discipline)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Disciplines.Add(discipline);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = discipline.DisciplineID }, discipline);
        }

        // POST: api/DisciplineData/DeleteDiscipline/5
       //curl -d ""http://localhost:49268/api/DisciplineData/DeleteDiscipline/3
        [ResponseType(typeof(Discipline))]
        [HttpPost]
        public IHttpActionResult DeleteDiscipline(int id)
        {
            Discipline discipline = db.Disciplines.Find(id);
            if (discipline == null)
            {
                return NotFound();
            }

            db.Disciplines.Remove(discipline);
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

        private bool DisciplineExists(int id)
        {
            return db.Disciplines.Count(e => e.DisciplineID == id) > 0;
        }
    }
}