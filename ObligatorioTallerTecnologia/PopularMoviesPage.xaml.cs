namespace ObligatorioTallerTecnologia;
using ObligatorioTallerTecnologia.Modelo;
using System.Collections.ObjectModel;

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

    private async void MostrarDetallesDePelicula_Clicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var movie = button?.BindingContext as Movie;

        if (movie != null)
        {
            // Implementar la l�gica para mostrar los detalles de la pel�cula
        }
    }

    private async void Pepito_Clicked(object sender, EventArgs e)
    {
        try
        {
            var photo = await MediaPicker.CapturePhotoAsync();
            if (photo != null)
            {
                var stream = await photo.OpenReadAsync();
                // Implementar la l�gica para manejar la foto capturada
            }
        }
        catch { }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var localizacion = await Geolocation.GetLastKnownLocationAsync();
        if (localizacion != null)
        {
            await DisplayAlert("Tu ubicaci�n es", $"Mi localizaci�n es: Latitud: {localizacion.Latitude}, Longitud: {localizacion.Longitude}", "Cerrar");
        }
    }
}