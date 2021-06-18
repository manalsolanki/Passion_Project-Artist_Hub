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
    public class ArtistController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static ArtistController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:49268/api/");
        }
        // GET: Artist/List
        public ActionResult List()
        {
            // Objective: Commmunicate with our Artist data api  to retrive a list of artists.
            // curl http://localhost:49268/api/ArtistData/ListArtists

            string url = "artistdata/listartists";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<ArtistDto> artists = response.Content.ReadAsAsync<IEnumerable<ArtistDto>>().Result;
            return View(artists);
        }

        // GET: Artist/Details/5
        public ActionResult Details(int id)
        {
            // Objective: Commmunicate with the Artist data api  to retrive a specific artist form with id.
            // curl http://localhost:49268/api/ArtistData/FindArtist/{id}

            string url = "artistdata/findartist/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            ArtistDto selectedArtist = response.Content.ReadAsAsync<ArtistDto>().Result;
            return View(selectedArtist);
        }

        // GET: Artist/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Artist/Create
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

        // GET: Artist/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Artist/Edit/5
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

        // GET: Artist/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Artist/Delete/5
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
