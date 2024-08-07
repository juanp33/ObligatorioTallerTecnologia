namespace ObligatorioTallerTecnologia
{
    using ObligatorioTallerTecnologia.Modelo;
    using System.Collections.ObjectModel;
    using Microsoft.Maui.Controls;

    public partial class PopularMoviesPage : ContentPage
    {
        private MovieService _movieService;
        private ObservableCollection<Movie> _movies;

        public PopularMoviesPage()
        {
            InitializeComponent();
            _movieService = new MovieService();
            _movies = new ObservableCollection<Movie>();
            MoviesCollectionView.ItemsSource = _movies;
            LoadMovies();
        }

        private async void LoadMovies()
        {
            var movies = await _movieService.GetPopularMoviesAsync();
            foreach (var movie in movies)
            {
                _movies.Add(movie);
            }
        }

        private async void OnImageTapped(object sender, EventArgs e)
        {
            var image = sender as Image;
            var movie = image?.BindingContext as Movie;
            if (movie != null)
            {
                var movieDetails = await _movieService.GetMovieDetailsAsync(movie.Id);
                if (movieDetails != null)
                {
                    await Navigation.PushAsync(new MovieDetailsPage(movieDetails));
                }
            }
        }
    }
}