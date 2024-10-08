using ObligatorioTallerTecnologia.Modelo;
using System;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace ObligatorioTallerTecnologia
{
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }

        public async void OnRegisterButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new RegistroUsuario());
        }

        public async void OnLoginButtonClicked(object sender, EventArgs args)
        {
            statusMessage.Text = "";

            if (!string.IsNullOrWhiteSpace(email.Text) &&
                !string.IsNullOrWhiteSpace(password.Text))
            {
                var user = App.UserRepository.GetUser(email.Text, password.Text);
                if (user != null)
                {
                    Preferences.Set("IsLoggedIn", true);
                    Preferences.Set("UserEmail", email.Text);
                    Preferences.Set("UserId", user.idUsuario.ToString());

                    statusMessage.Text = "Inicio de sesión exitoso.";

                    if (Application.Current.MainPage is AppShell appShell)
                    {
                        appShell.UpdateShell();
                    }
                    await DisplayAlert("Login exitoso", "Se logueo correctamente", "OK");
                    await Shell.Current.GoToAsync("///MainPage");
                }
                else
                {
                    statusMessage.Text = "Correo o contraseña incorrectos.";
                }
            }
            else
            {
                statusMessage.Text = "Todos los campos son obligatorios.";
            }
        }

        public async void OnFingerprintButtonClicked(object sender, EventArgs args)
        {
            try
            {
                var availability = await CrossFingerprint.Current.IsAvailableAsync(true);
                if (!availability)
                {
                    statusMessage.Text = "Autenticación biométrica no disponible.";
                    return;
                }

                var authResult = await CrossFingerprint.Current.AuthenticateAsync(new AuthenticationRequestConfiguration("Autenticación con Huella Digital", "Use su huella digital para autenticarse"));

                if (authResult.Authenticated)
                {
                    
                    var user = App.UserRepository.GetAll().LastOrDefault();

                    if (user != null)
                    {
                        
                        Preferences.Set("IsLoggedIn", true);
                        Preferences.Set("UserEmail", user.email);
                        Preferences.Set("UserId", user.idUsuario.ToString());

                        statusMessage.Text = "Autenticación exitosa.";

                        if (Application.Current.MainPage is AppShell appShell)
                        {
                            appShell.UpdateShell();
                        }

                        await Shell.Current.GoToAsync($"//MainPage");
                    }
                    else
                    {
                        statusMessage.Text = "No se encontró ningún usuario registrado.";
                    }
                }
                else
                {
                    statusMessage.Text = "Autenticación fallida.";
                }
            }
            catch (Exception ex)
            {
                statusMessage.Text = $"Error: {ex.Message}";
            }
        }

    }
}
