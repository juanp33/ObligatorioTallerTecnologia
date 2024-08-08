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

                    statusMessage.Text = "Inicio de sesi�n exitoso.";

                    if (Application.Current.MainPage is AppShell appShell)
                    {
                        appShell.UpdateShell();
                    }
                    
                    await Shell.Current.GoToAsync("///MainPage");
                }
                else
                {
                    statusMessage.Text = "Correo o contrase�a incorrectos.";
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
                    statusMessage.Text = "Autenticaci�n biom�trica no disponible.";
                    return;
                }

                var authResult = await CrossFingerprint.Current.AuthenticateAsync(new AuthenticationRequestConfiguration("Autenticaci�n con Huella Digital", "Use su huella digital para autenticarse"));

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

                            statusMessage.Text = "Autenticaci�n exitosa.";

                            // Enviar mensaje de cambio de sesi�n
                            MessagingCenter.Send(this, "SessionChanged");

                            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                        }
                        else
                        {
                            statusMessage.Text = "No se encontr� un usuario asociado.";
                        }
                    }
                    else
                    {
                        statusMessage.Text = "No se encontr� un usuario registrado con esta huella.";
                    }
                }
                else
                {
                    statusMessage.Text = "Autenticaci�n fallida.";
                }
            }
            catch (Exception ex)
            {
                statusMessage.Text = $"Error: {ex.Message}";
            }
        }
    }
}
