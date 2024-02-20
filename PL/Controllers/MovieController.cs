using Microsoft.AspNetCore.Mvc;
using ML;
using System.Net.Http;
namespace PL.Controllers
{
    public class MovieController : Controller
    {
        private readonly IConfiguration _configuration;
        public MovieController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public ActionResult GetAll(ML.Root root)
        {
            root.Results = new List<ML.Result>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration["WebApi"]);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_configuration["AccessToken"]}");
                var responseTask = client.GetAsync("movie/popular");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ML.Root>();
                    readTask.Wait();
                    root = readTask.Result;
                    root.Results = readTask.Result.Results;
                }
                else
                {
                    ViewBag.Mensaje = "Ocurrio un error al consultar la información";
                }
            }
            return View(root);
        }
        [HttpPost]
        public ActionResult AddMovie(int IdMovie, int btnFav)
        {
            ML.FavoriteResult favResult = new ML.FavoriteResult();
            bool favorite = false;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration["WebApi"]);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_configuration["AccessToken"]}");
                if (btnFav == 1)
                {
                    favorite = true;
                }
                else
                {
                    favorite = false;
                }
                ML.AddFavorite addFavorite = new ML.AddFavorite();
                addFavorite.MediaType = "movie";
                addFavorite.MediaId = IdMovie;
                addFavorite.Favorite = favorite;
                //var anonymousObject = new { mediaType = "movie", mediaId = IdMovie, favorite = favorite };
                var postTask = client.PostAsJsonAsync($"account/20961193/favorite?{_configuration["SessionId"]}", addFavorite);
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ML.FavoriteResult>();
                    readTask.Wait();
                    favResult = readTask.Result;
                    favResult.StatusMessage = readTask.Result.StatusMessage;
                    favResult.Success = readTask.Result.Success;
                    favResult.StatusCode = readTask.Result.StatusCode;
                    //status code agregar "1" elminiar "13"
                    if (favResult.StatusCode == 1)
                    {
                        ViewBag.Mensaje = "Se agrego a favoritos";
                    }
                    if (favResult.StatusCode == 13)
                    {
                        ViewBag.Mensaje = "Se elimino de favoritos";
                    }

                }
                else
                {
                    ViewBag.Mensaje = "Ocurrio un error al consultar la información" + favResult.StatusMessage;
                }
            }
            return PartialView("Modal");
        }
        [HttpGet]
        public ActionResult GetFavorite(ML.Root root)
        {
            root.Results = new List<ML.Result>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration["WebApi"]);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_configuration["AccessToken"]}");
                var responseTask = client.GetAsync($"account/20961193/favorite/movies?{_configuration["SessionId"]}");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ML.Root>();
                    readTask.Wait();
                    root = readTask.Result;
                    root.Results = readTask.Result.Results;
                }
                else
                {
                    ViewBag.Mensaje = "Ocurrio un error al consultar la información";
                }
            }
            return View(root);
        }
    }
}
