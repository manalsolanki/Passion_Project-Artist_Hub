using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Web.Script.Serialization;
using ArtistsHub.Models;
using ArtistsHub.Models.ViewModel;
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
            DetailArtist viewModel = new DetailArtist();
            // Objective: Commmunicate with the Artist data api  to retrive a specific artist form with id.
            // curl http://localhost:49268/api/ArtistData/FindArtist/{id}

            string url = "artistdata/findartist/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            ArtistDto selectedArtist = response.Content.ReadAsAsync<ArtistDto>().Result;
            viewModel.selectedArtist = selectedArtist;
            Debug.WriteLine(selectedArtist);


            // Get all the art forms related to this artist.
            url = "ArtFormData/ListArtFormsForArtist/"+ id;
            Debug.WriteLine(url);
            response = client.GetAsync(url).Result;
            IEnumerable<ArtFormDto> artForms = response.Content.ReadAsAsync<IEnumerable<ArtFormDto>>().Result;
            viewModel.relatedArtForms = artForms;


            // Get all the list of artform which an atist is not interested in(Available Artforms).
            url = "ArtFormData/ListArtFormsNotInterestedByArtist/"+id;
            response = client.GetAsync(url).Result;
            IEnumerable<ArtFormDto> availableArtForms = response.Content.ReadAsAsync<IEnumerable<ArtFormDto>>().Result;
            
            viewModel.availableArtForms = availableArtForms;


            return View(viewModel);
        }

        // POST : Artist/Associate/{artistId}
        [HttpPost]
        public ActionResult Associate(int id, int ArtFormId)
        {
            Debug.WriteLine("Associated artform" + ArtFormId);

            //call our api to associate artist with artforms.
            string url = "ArtistData/AssociateArtFormsWithArtist/" + id+"/"+ArtFormId;
            Debug.WriteLine("URL " + url);
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(response);
            return RedirectToAction("Details/" + id);
        }




        // POST :Artist/UnAssociate/{id}?ArtFormID={ArtFormID}
        [HttpGet]
        public ActionResult UnAssociate(int id, int ArtFormId)
        {
            //call our api to unassociate artist with artforms.
            string url = "ArtistData/UnAssociateArtFormsWithArtist/" + id + "/" + ArtFormId;
            Debug.WriteLine("URL " + url);
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(response);
            return RedirectToAction("Details/" + id);
        }


        //GET: ArtForm/Error
        public ActionResult Error()
        {
            return View();
        }

        // GET: Artist/New
        [Authorize]
        public ActionResult New()
        {
            return View();
        }

        // POST: Artist/Create
        [HttpPost]
        public ActionResult Create(Artist artist)
        {
            string url = "artistdata/addArtist";
            //objective: Communicate with artist data controller to add a new artist.
            //curl - d @artform.json - H "Content-type:application/json" http://localhost:49268/api/ArtistData/AddArtist            string url = "artformdata/addArtForm";

            string jsonpayload = jss.Serialize(artist);

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

        // GET: Artist/Update/5
        public ActionResult Update(int id)
        {
            // Objective: Commmunicate with the Artist data api  to retrive a specific artist form with id.
            // curl http://localhost:49268/api/ArtistData/FindArtist/{id}

            string url = "artistdata/findartist/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            ArtistDto selectedArtist = response.Content.ReadAsAsync<ArtistDto>().Result;
            return View(selectedArtist);
        }

        // POST: Artist/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Artist artist ,HttpPostedFileBase ProfilePic)
        {
            // objective : Communicate with the artist controller to update the artist having specific id.
            //curl -d @artist.json -H "Content-type:application/json" http://localhost:49268/api/ArtistData/UpdateArtist/3

            string url = "artistdata/UpdateArtist/" + id;
            artist.ArtistID = id;
            string jsonpayload = jss.Serialize(artist);
            Debug.WriteLine(jsonpayload);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode && ProfilePic != null)
            {
                //Updating the profile picture as a separate request
                Debug.WriteLine("Calling Update Image method.");
                //Send over image data for artist
                url = "ArtistData/UploadArtistPic/" + id;
                //Debug.WriteLine("Received Animal Picture "+AnimalPic.FileName);

                MultipartFormDataContent requestcontent = new MultipartFormDataContent();
                HttpContent imagecontent = new StreamContent(ProfilePic.InputStream);
                requestcontent.Add(imagecontent, "artistProfilePic", ProfilePic.FileName);
                response = client.PostAsync(url, requestcontent).Result;
                return RedirectToAction("Error");
            }
            else if (response.IsSuccessStatusCode)
            {
                //No image upload, but update still successful
                return RedirectToAction("List");
            }

            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Artist/DeleteConfirmation/5
        public ActionResult DeleteConfirmation(int id)
        {
            // Objective: Commmunicate with the Artist data api  to retrive a specific artist form with id.
            // curl http://localhost:49268/api/ArtistData/FindArtist/{id}

            string url = "artistdata/findartist/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            ArtistDto selectedArtist = response.Content.ReadAsAsync<ArtistDto>().Result;
            return View(selectedArtist);
        }

        // POST: Artist/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            // Objective : Connects with artist wb spi to delete a artist with {id}.
            // curl - d "" http://localhost:49268/api/ArtistData/DeleteArtist/3

            string url = "artistdata/deleteArtist/" + id;
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
