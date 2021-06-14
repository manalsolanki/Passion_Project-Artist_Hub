using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Web.Script.Serialization;
using ArtistsHub.Models;
using System.Diagnostics;

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

            string url = "DisciplineData/ListDisciplines";
            HttpResponseMessage response = client.GetAsync(url).Result;
            Debug.WriteLine(response);
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

            DisciplineDto selectedDiscipline = response.Content.ReadAsAsync<DisciplineDto>().Result;
            return View(selectedDiscipline);
        }
        //GET: Discipline/Error
        public ActionResult Error()
        {
            return View();
        }

        // GET: Discipline/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Discipline/Create
        [HttpPost]
        public ActionResult Create(Discipline discipline)
        {
            string url = "disciplinedata/addDiscipline";
            //objective: add a new artform into the system using the API
            //curl - d @artform.json - H "Content-type:application/json" http://localhost:49268/api/ArtFormData/AddArtForm            string url = "artformdata/addArtForm";

            string jsonpayload = jss.Serialize(discipline);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Discipline/Update/5
        public ActionResult Update(int id)
        {
            string url = "DisciplineData/FindDiscipline/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            DisciplineDto selectedDiscipline = response.Content.ReadAsAsync<DisciplineDto>().Result;
            return View(selectedDiscipline);
        }

        // POST: Discipline/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Discipline discipline)
        {
            string url = "disciplineData/UpdateDiscipline/" + id;
            discipline.DisciplineID = id;
            string jsonpayload = jss.Serialize(discipline);
            //Debug.WriteLine(jsonpayload);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
           
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Discipline/DeleteConfirmation/5
        public ActionResult DeleteConfirmation(int id)
        {
            string url = "DisciplineData/FindDiscipline/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            DisciplineDto selectedDiscipline = response.Content.ReadAsAsync<DisciplineDto>().Result;
            return View(selectedDiscipline);
        }

        // POST: Discipline/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "disciplinedata/deleteDiscipline/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
