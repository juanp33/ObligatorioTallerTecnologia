using ObligatorioTallerTecnologia.Modelo;
using System;
using Microsoft.Maui.Controls;

namespace ObligatorioTallerTecnologia
{
    public partial class Perfil : ContentPage
    {
        public Perfil()
        {
            InitializeComponent();
            LoadUserProfile();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadUserProfile();
        }

        private void LoadUserProfile()
        {
            // Obtén el usuario actual de las preferencias o de otra fuente
            var userEmail = Preferences.Get("UserEmail", string.Empty);
            if (!string.IsNullOrEmpty(userEmail))
            {
                var user = App.UserRepository.GetUserByEmail(userEmail);
                if (user != null)
                {
                    userNameLabel.Text = user.nombreUsuario;
                    userEmailLabel.Text = user.email;

                    if (!string.IsNullOrEmpty(user.imagenFoto))
                    {
                        profileImage.Source = ImageSource.FromFile(user.imagenFoto);
                    }
                }
            }
        }

        private async void OnLogoutButtonClicked(object sender, EventArgs args)
        {

            Preferences.Remove("UserEmail");
            Preferences.Remove("UserEmail");
            Preferences.Remove("IsLoggedIn");

            ((AppShell)Application.Current.MainPage).UpdateShell();
            await Shell.Current.GoToAsync("///MainPage");
        }
    }
}
