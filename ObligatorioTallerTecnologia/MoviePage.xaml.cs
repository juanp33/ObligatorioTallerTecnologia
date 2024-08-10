
using Microsoft.Maui.Controls.Maps;
using Newtonsoft.Json.Linq;
using ObligatorioTallerTecnologia.Modelo;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace ObligatorioTallerTecnologia
{
    public partial class MoviesPage : ContentPage
    {
        private readonly MovieService _movieService;
        private Location userLocation;
        private Polyline routePolyline;

        public MoviesPage()
        {
            InitializeComponent();
            _movieService = new MovieService();
            LoadPopularMovies();
        }
       
        private async Task LoadPopularMovies()
        {
            var movies = await _movieService.GetPopularMoviesAsync();
            MoviesListView.ItemsSource = movies;
        }
        private async void OnSearchButtonPressed(object sender, EventArgs e)
        {
            string query = BusquedaBar.Text;

            if (!string.IsNullOrEmpty(query))
            {
                var moviesByName = await _movieService.SearchMoviesAsync(query);
                var moviesByActor = await _movieService.SearchMoviesByActorAsync(query);


                var combinedMovies = moviesByName.Concat(moviesByActor).GroupBy(m => m.Id).Select(g => g.First()).ToList();

                MoviesListView.ItemsSource = combinedMovies;
            }
        }
        private async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is Movie selectedMovie)
            {
                var movieDetails = await _movieService.GetMovieDetailsAsync(selectedMovie.Id);
                if (movieDetails != null)
                {
                    await Navigation.PushAsync(new MovieDetailsPage(movieDetails));
                }
            }

             
             ((ListView)sender).SelectedItem = null;
        }
    }
}