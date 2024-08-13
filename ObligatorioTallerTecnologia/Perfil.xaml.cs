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
           
            var userEmail = Preferences.Get("UserEmail", string.Empty);
            if (!string.IsNullOrEmpty(userEmail))
            {
                var user = App.UserRepository.GetUserByEmail(userEmail);
                if (user != null)
                {
                    userNameLabel.Text = "Nombre: " + user.nombreUsuario;
                    userEmailLabel.Text = "Email: " + user.email;
                    userPhoneLabel.Text = "Telefono: " + user.telefono;

                    if (!string.IsNullOrEmpty(user.imagenFoto))
                    {
                        profileImage.Source = ImageSource.FromFile(user.imagenFoto);
                    }
                    else
                    {
                        profileImage.Source = ImageSource.FromFile("fotosuarez.jpg");
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
            await DisplayAlert("Sesión finalizada", "Se deslogueo correctamente", "OK");
            await Shell.Current.GoToAsync("///MainPage");
        }
    }
}
