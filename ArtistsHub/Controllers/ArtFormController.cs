using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using ArtistsHub.Models;
using ArtistsHub.Models.ViewModel;
using System.Web.Script.Serialization;


namespace ArtistsHub.Controllers
{
    public class ArtFormController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static ArtFormController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                //cookies are manually set in RequestHeader
                UseCookies = false
            };
            client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:49268/api/");
        }

        /// <summary>
        /// Grabs the authentication cookie sent to this controller.
        /// For proper WebAPI authentication, you can send a post request with login credentials to the WebAPI and log the access token from the response. The controller already knows this token, so we're just passing it up the chain.
        /// 
        /// Here is a descriptive article which walks through the process of setting up authorization/authentication directly.
        /// https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/individual-accounts-in-web-api
        /// </summary>
        private void GetApplicationCookie()
        {
            string token = "";
            //HTTP client is set up to be reused, otherwise it will exhaust server resources.
            //This is a bit dangerous because a previously authenticated cookie could be cached for
            //a follow-up request from someone else. Reset cookies in HTTP client before grabbing a new one.
            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;

            //collect token as it is submitted to the controller
            //use it to pass along to the WebAPI.
            Debug.WriteLine("Token Submitted is : " + token);
            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
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
            DetailArtForm viewModel = new DetailArtForm();
            // Objective: Commmunicate with the Artform data api  to retrive a specific art form with id.
            // curl http://localhost:49268/api/ArtFormData/FindArtForm/{id}

            string url = "artformdata/findartform/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            ArtFormDto selectedArtForm = response.Content.ReadAsAsync<ArtFormDto>().Result;
            viewModel.selectedArtForm = selectedArtForm;

            // Showing details of artist related to that artform.

            url = "artistdata/ListArtistForArtForm/" + id;
            response = client.GetAsync(url).Result;

            IEnumerable<ArtistDto> relatedArtists = response.Content.ReadAsAsync<IEnumerable<ArtistDto>>().Result;

            viewModel.relatedArtist = relatedArtists;
            return View(viewModel);
        }

        //GET: ArtForm/Error
        public ActionResult Error()
        {
            return View();
        }
        [Authorize]
        // GET: ArtForm/New
        public ActionResult New()
        {

            //information about all discipline 
            //GET api/disciplinedata/listdisciplines

            string url = "disciplinedata/listdisciplines";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<DisciplineDto> DisciplineOptions = response.Content.ReadAsAsync<IEnumerable<DisciplineDto>>().Result;

            return View(DisciplineOptions);
           
        }

        // POST: ArtForm/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(ArtForm artform)
        {
            GetApplicationCookie();
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
        [Authorize]
        public ActionResult Update(int id)
        {
            UpdateArtForm viewModel = new UpdateArtForm();

            // Objective: Commmunicate with the Artform data api  to retrive a specific art form with id.
            // curl http://localhost:49268/api/ArtFormData/FindArtForm/{id}

            string url = "artformdata/findartform/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            ArtFormDto selectedArtForm = response.Content.ReadAsAsync<ArtFormDto>().Result;
            viewModel.SelectedArtForm = selectedArtForm;

            //information about all discipline 
            //GET api/disciplinedata/listdisciplines

            url = "disciplinedata/listdisciplines";
            response = client.GetAsync(url).Result;
            IEnumerable<DisciplineDto> DisciplineOptions = response.Content.ReadAsAsync<IEnumerable<DisciplineDto>>().Result;

            viewModel.DisciplinesOptions= DisciplineOptions;


            return View(viewModel);
        }

        // POST: ArtForm/Edit/5
    
        [HttpPost]
        [Authorize]
        public ActionResult Edit(int id, ArtForm artForm)
        {
            GetApplicationCookie();// for authorization
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
        [Authorize]
        public ActionResult DeleteConfirmation(int id)
        {
            string url = "artformdata/findartform/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ArtFormDto selectedArtForm = response.Content.ReadAsAsync<ArtFormDto>().Result;
            return View(selectedArtForm);
        }

        // POST: ArtForm/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();
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
