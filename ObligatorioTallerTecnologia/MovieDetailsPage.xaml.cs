namespace ObligatorioTallerTecnologia
{
    using ObligatorioTallerTecnologia.Modelo;
    using Microsoft.Maui.Controls;
    using System;
    using System.Collections.ObjectModel;
    using Newtonsoft.Json;

    public partial class MovieDetailsPage : ContentPage
    {
        private int _currentIndex;
        private Usuario _currentUser;
        private Movie _movieDetails;
        public MovieDetailsPage(Movie movieDetails)
        {
            InitializeComponent();
            _movieDetails = movieDetails;
            BindingContext = movieDetails;
            _currentIndex = 0;
            LoadCurrentUser();
            UpdateFavoriteButtonText();
        }

        private void OnLeftArrowTapped(object sender, EventArgs e)
        {
            ScrollCollectionView(-1);
        }

        private void OnRightArrowTapped(object sender, EventArgs e)
        {
            ScrollCollectionView(1);
        }

        private void ScrollCollectionView(int direction)
        {
            var castList = (BindingContext as Movie)?.Credits?.Cast;
            if (castList != null && castList.Count > 0)
            {
                _currentIndex = Math.Max(0, Math.Min(castList.Count - 1, _currentIndex + direction));
                ActorsCollectionView.ScrollTo(_currentIndex, position: ScrollToPosition.Center, animate: true);
            }
        }
        private void LoadCurrentUser()
        {
            var userEmail = Preferences.Get("UserEmail", string.Empty);
            if (!string.IsNullOrEmpty(userEmail))
            {
                _currentUser = App.UserRepository.GetUserByEmail(userEmail);
            }
            else
            {
                _currentUser = null;
            }
        }

        private void UpdateFavoriteButtonText()
        {
            if (_currentUser == null)
            {
                FavoriteButton.IsEnabled = false;
                FavoriteButton.Text = "Inicia sesi�n para a�adir a Favoritos";
                return;
            }

            string movieIdString = _movieDetails.Id.ToString();
            var favoriteMovies = App.UserRepository.GetFavoriteMovies(_currentUser.idUsuario);
            if (favoriteMovies.Contains(movieIdString))
            {
                FavoriteButton.Text = "Eliminar de Favoritos";
            }
            else
            {
                FavoriteButton.Text = "Marcar como Favorito";
            }
        }

        private void OnFavoriteButtonClicked(object sender, EventArgs e)
        {
            if (_currentUser == null)
            {
                DisplayAlert("Iniciar sesi�n", "Por favor, inicia sesi�n para poder a�adir esta pel�cula a tus favoritos.", "Aceptar");
                return;
            }

            string movieIdString = _movieDetails.Id.ToString();
            var favoriteMovies = App.UserRepository.GetFavoriteMovies(_currentUser.idUsuario);

            if (favoriteMovies.Contains(movieIdString))
            {
                App.UserRepository.RemoveFavoriteMovie(_currentUser.idUsuario, movieIdString);
                FavoriteButton.Text = "Marcar como Favorito";
            }
            else
            {
                App.UserRepository.AddFavoriteMovie(_currentUser.idUsuario, movieIdString);
                FavoriteButton.Text = "Eliminar de Favoritos";
            }
        }

    }
}
