﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using ArtistsHub.Models;
using System.Web.Script.Serialization;


namespace ArtistsHub.Controllers
{
    public class ArtFormController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static ArtFormController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:49268/api/");
        }
        // GET: ArtForm/List
        public ActionResult List()
        {
            // Objective: Commmunicate with our Artform data api  to retrive a list of art forms.
            // curl http://localhost:49268/api/ArtFormData/ListArtForms

            string url = "artformdata/listartforms";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<ArtFormDto> artForms = response.Content.ReadAsAsync<IEnumerable<ArtFormDto>>().Result;
            return View(artForms);
        }

        // GET: ArtForm/Details/5
        public ActionResult Details(int id)
        {
            // Objective: Commmunicate with the Artform data api  to retrive a specific art form with id.
            // curl http://localhost:49268/api/ArtFormData/FindArtForm/{id}

            string url = "artformdata/findartform/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            ArtFormDto selectedArtForm = response.Content.ReadAsAsync<ArtFormDto>().Result;
            return View(selectedArtForm);
        }

        //GET: ArtForm/Error
        public ActionResult Error()
        {
            return View();
        }

        // GET: ArtForm/New
        public ActionResult New()
        {
            return View();
        }

        // POST: ArtForm/Create
        [HttpPost]
        public ActionResult Create(ArtForm artform)
        {
            string url = "artformdata/addArtform";
            //objective: add a new artform into the system using the API
            //curl - d @artform.json - H "Content-type:application/json" http://localhost:49268/api/ArtFormData/AddArtForm            string url = "artformdata/addArtForm";

            string jsonpayload = jss.Serialize(artform);
            
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

        // GET: ArtForm/Update/5
        public ActionResult Update(int id)
        {
            // Objective: Commmunicate with the Artform data api  to retrive a specific art form with id.
            // curl http://localhost:49268/api/ArtFormData/FindArtForm/{id}

            string url = "artformdata/findartform/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            ArtFormDto selectedArtForm = response.Content.ReadAsAsync<ArtFormDto>().Result;
            return View(selectedArtForm);
        }

        // POST: ArtForm/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, ArtForm artForm)
        {
            string url = "artformdata/UpdateArtForm/" + id;
            artForm.ArtFormID = id;
            string jsonpayload = jss.Serialize(artForm);
            Debug.WriteLine(jsonpayload);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: ArtForm/DeleteConfirmation/5
        public ActionResult DeleteConfirmation(int id)
        {
            string url = "artformdata/findartform/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ArtFormDto selectedArtForm = response.Content.ReadAsAsync<ArtFormDto>().Result;
            return View(selectedArtForm);
        }

        // POST: ArtForm/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "artformdata/deleteArtForm/" + id;
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
