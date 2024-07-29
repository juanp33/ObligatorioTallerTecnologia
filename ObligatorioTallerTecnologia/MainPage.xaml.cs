﻿using ObligatorioTallerTecnologia.Modelo;
using System.Linq.Expressions;

namespace ObligatorioTallerTecnologia
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        private MovieService _movieService;

        public MainPage()
        {
            InitializeComponent();
            _movieService = new MovieService();
            LoadMovies();
        }


        private async void LoadMovies()
        {
            var movies = await _movieService.GetPopularMoviesAsync();
            MoviesListView.ItemsSource = movies;
        }

        private async void MostrarDetallesDePelicula_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;

            var movie = button?.BindingContext as Movie;

            if (movie != null)
            {
                
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
                }     
            }
               catch { }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var localizacion = await Geolocation.GetLastKnownLocationAsync();
            if (localizacion != null)
            {
                await DisplayAlert("Tu ubicacion es", "Mi localizacion es: Latitud: " + localizacion.Latitude ,"Cerrar");
            }
        }
    }

}
