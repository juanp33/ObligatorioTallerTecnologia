using ObligatorioTallerTecnologia.Modelo;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace ObligatorioTallerTecnologia
{
    public partial class FavoriteMoviesPage : ContentPage
    {
        private readonly MovieService _movieService;
        private ObservableCollection<Movie> _favoriteMovies;

        public FavoriteMoviesPage()
        {
            InitializeComponent();
            _movieService = new MovieService();
            _favoriteMovies = new ObservableCollection<Movie>();
            FavoritesListView.ItemsSource = _favoriteMovies;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadFavoriteMovies();
        }

        private async Task LoadFavoriteMovies()
        {
            var userEmail = Preferences.Get("UserEmail", string.Empty);
            var currentUser = App.UserRepository.GetUserByEmail(userEmail);

            if (currentUser != null)
            {
                
                var favoriteMovies = App.UserRepository.GetFavoriteMovies(currentUser.idUsuario);

                _favoriteMovies.Clear(); 
                foreach (var favoriteMovie in favoriteMovies)
                {
                    var movie = await _movieService.GetMovieDetailsAsync(int.Parse(favoriteMovie));
                    if (movie != null)
                    {
                        _favoriteMovies.Add(movie);
                    }
                }
            }
            else
            {
                await DisplayAlert("Error", "No se pudo cargar el usuario actual.", "OK");
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
