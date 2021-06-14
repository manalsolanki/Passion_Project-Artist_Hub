using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Web.Script.Serialization;
using ArtistsHub.Models;

namespace ArtistsHub.Controllers
{

    public class DisciplineController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static DisciplineController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:49268/api/");
        }
        // GET: Discipline/List
        public ActionResult List()
        {
            // Objective: Commmunicate with our Discipline data api  to retrive a list of all disciplines.
            // curl http://localhost:49268/api/DisciplineData/ListDiscipline

            string url = "DisciplineData/ListDiscipline";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<DisciplineDto> disciplines = response.Content.ReadAsAsync<IEnumerable<DisciplineDto>>().Result;
            return View(disciplines);
        }

        // GET: Discipline/Details/5
        public ActionResult Details(int id)
        {
            // Objective: Commmunicate with the Discipline data api  to retrive a specific discipline with id.
            // curl http://localhost:49268/api/DisciplineData/FindDiscipline/{id}

            string url = "DisciplineData/FindDiscipline/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Discipline selectedDiscipline = response.Content.ReadAsAsync<Discipline>().Result;
            return View(selectedDiscipline);
        }

        // GET: Discipline/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Discipline/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Discipline/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Discipline/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Discipline/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Discipline/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
