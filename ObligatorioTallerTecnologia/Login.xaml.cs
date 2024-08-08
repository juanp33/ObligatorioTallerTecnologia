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
                    var userId = Preferences.Get("UserId", string.Empty);
                    if (!string.IsNullOrEmpty(userId))
                    {
                        var user = App.UserRepository.GetUserById(int.Parse(userId));
                        if (user != null)
                        {
                            Preferences.Set("IsLoggedIn", true);
                            Preferences.Set("UserEmail", user.email);

                            statusMessage.Text = "Autenticación exitosa.";

                            // Enviar mensaje de cambio de sesión
                            MessagingCenter.Send(this, "SessionChanged");

                            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                        }
                        else
                        {
                            statusMessage.Text = "No se encontró un usuario asociado.";
                        }
                    }
                    else
                    {
                        statusMessage.Text = "No se encontró un usuario registrado con esta huella.";
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
