using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

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
                request.AddParameter("language", "en-US");
                request.AddParameter("page", 1);

                var response = await _client.GetAsync(request);

                if (!response.IsSuccessful)
                {
                    return new List<Movie>();
                }

                var movieResponse = JsonConvert.DeserializeObject<MovieResponse>(response.Content);
                return movieResponse?.Results ?? new List<Movie>();
            }
        }

        public class MovieResponse
        {
            [JsonProperty("results")]
            public List<Movie> Results { get; set; }
        }

        
       
        public class Movie
        {
            private const string UrlBaseDeImagen = "https://image.tmdb.org/t/p/w500";

            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("overview")]
            public string Overview { get; set; }

            [JsonProperty("release_date")]
            public string ReleaseDate { get; set; }

            [JsonProperty("poster_path")]
            public string PosterPath { get; set; }

            public string UrlImgCompleta => string.IsNullOrEmpty(PosterPath) ? null : $"{UrlBaseDeImagen}{PosterPath}";

            public bool IsDescriptionExpanded { get; set; } = false;

            public string ShortOverview => Overview.Length > 150 ? Overview.Substring(0, 100) + "..." : Overview;

            public string ToggleText => IsDescriptionExpanded ? "Ver menos" : "Ver más";
        }
    
    

}
