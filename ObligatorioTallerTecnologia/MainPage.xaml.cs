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

        private void Pepito_Clicked(object sender, EventArgs e)
        {
            try
            {
                var photo = MediaPicker.CapturePhotoAsync();
                if (photo != null)
                {
                    var stream = photo.Result.OpenReadAsync();
                }     
            }
               catch { }
        }
    }

}
