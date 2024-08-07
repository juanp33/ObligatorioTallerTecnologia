using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObligatorioTallerTecnologia.Modelo
{
    internal class MovieService
    {
        private readonly RestClient _client;
        private const string ApiKey = "ea58a3848d2c2e6bf2906208c725d9d1";
        private const string BaseUrl = "https://api.themoviedb.org/3";

        public MovieService()
        {
            _client = new RestClient(BaseUrl);
        }
        public async Task<List<Movie>> GetPopularMoviesAsync()
        {
            var request = new RestRequest("movie/popular", Method.Get);
            request.AddParameter("api_key", ApiKey);
            request.AddParameter("language", "es-ES");
            request.AddParameter("page", 1);

            var response = await _client.GetAsync(request);

            if (!response.IsSuccessful)
            {
                return new List<Movie>();
            }

            var movieResponse = JsonConvert.DeserializeObject<MovieResponse>(response.Content);
            return movieResponse?.Results ?? new List<Movie>();
        }

        public async Task<List<Movie>> GetAllMoviesAsync()
        {
            var request = new RestRequest("movie", Method.Get);
            request.AddParameter("api_key", ApiKey);
            request.AddParameter("language", "es-ES");
            request.AddParameter("page", 1);

            var response = await _client.GetAsync(request);

            if (!response.IsSuccessful)
            {
                return new List<Movie>();
            }

            var movieResponse = JsonConvert.DeserializeObject<MovieResponse>(response.Content);
            return movieResponse?.Results ?? new List<Movie>();
        }
        public async Task<List<Movie>> SearchMoviesAsync(string query)
        {
            var request = new RestRequest("search/movie", Method.Get);
            request.AddParameter("api_key", ApiKey);
            request.AddParameter("language", "es-ES");
            request.AddParameter("query", query);
            request.AddParameter("page", 1);

            var response = await _client.GetAsync(request);

            if (!response.IsSuccessful)
            {
                return new List<Movie>();
            }

            var movieResponse = JsonConvert.DeserializeObject<MovieResponse>(response.Content);
            return movieResponse?.Results ?? new List<Movie>();
        }

        public async Task<List<Movie>> SearchMoviesByActorAsync(string actorName)
        {
            // Primero, obtener el ID del actor
            var actorRequest = new RestRequest("search/person", Method.Get);
            actorRequest.AddParameter("api_key", ApiKey);
            actorRequest.AddParameter("language", "es-ES");
            actorRequest.AddParameter("query", actorName);
            actorRequest.AddParameter("page", 1);

            var actorResponse = await _client.GetAsync(actorRequest);

            if (!actorResponse.IsSuccessful)
            {
                return new List<Movie>();
            }

            var actorSearchResponse = JsonConvert.DeserializeObject<PersonSearchResponse>(actorResponse.Content);
            var actor = actorSearchResponse?.Results?.FirstOrDefault();

            if (actor == null)
            {
                return new List<Movie>();
            }

            // Ahora, buscar películas por el ID del actor
            var movieRequest = new RestRequest($"discover/movie", Method.Get);
            movieRequest.AddParameter("api_key", ApiKey);
            movieRequest.AddParameter("language", "es-ES");
            movieRequest.AddParameter("with_cast", actor.Id);

            var movieResponse = await _client.GetAsync(movieRequest);

            if (!movieResponse.IsSuccessful)
            {
                return new List<Movie>();
            }

            var movieListResponse = JsonConvert.DeserializeObject<MovieResponse>(movieResponse.Content);
            return movieListResponse?.Results ?? new List<Movie>();
        }
        public async Task<Movie> GetMovieDetailsAsync(int movieId)
        {
            var request = new RestRequest($"movie/{movieId}", Method.Get);
            request.AddParameter("api_key", ApiKey);
            request.AddParameter("language", "es-ES");
            request.AddParameter("append_to_response", "videos,credits");

            var response = await _client.GetAsync(request);

            if (!response.IsSuccessful)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<Movie>(response.Content);
        }

     
    }

    public class MovieResponse
    {
        [JsonProperty("results")]
        public List<Movie> Results { get; set; }
    }
    public class PersonSearchResponse
    {
        [JsonProperty("results")]
        public List<Person> Results { get; set; }
    }

    public class Person
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
    public class Movie
    {
        private const string UrlBaseDeImagen = "https://image.tmdb.org/t/p/w500";

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("overview")]
        public string Overview { get; set; }

        [JsonProperty("release_date")]
        public string ReleaseDate { get; set; }

        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }

        [JsonProperty("videos")]
        public VideoResponse Videos { get; set; }

        [JsonProperty("credits")]
        public Credits Credits { get; set; }

        

        public string UrlImgCompleta => string.IsNullOrEmpty(PosterPath) ? null : $"{UrlBaseDeImagen}{PosterPath}";

        public string TrailerUrl
        {
            get
            {
                var trailer = Videos?.Results?.FirstOrDefault(v => v.Type == "Trailer" && v.Site == "YouTube");
                return trailer != null ? $"https://www.youtube.com/embed/{trailer.Key}" : null;
            }
        }
    }

    public class VideoResponse
    {
        [JsonProperty("results")]
        public List<Video> Results { get; set; }
    }

    public class Video
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("site")]
        public string Site { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class Credits
    {
        [JsonProperty("cast")]
        public List<Cast> Cast { get; set; }
    }

    public class Cast
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("character")]
        public string Character { get; set; }

        [JsonProperty("profile_path")]
        public string ProfilePath { get; set; }

        public string ProfileImg => string.IsNullOrEmpty(ProfilePath) ? null : $"https://image.tmdb.org/t/p/w500{ProfilePath}";
    }

 
}
