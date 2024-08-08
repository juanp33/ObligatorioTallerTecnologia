using ObligatorioTallerTecnologia.Modelo;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;


namespace ObligatorioTallerTecnologia
{
    public partial class RegistroUsuario : ContentPage
    {
        public RegistroUsuario()
        {
            InitializeComponent();
        }

        public async void OnNewButtonClicked(object sender, EventArgs args)
        {
            statusMessage.Text = "";

            if (!string.IsNullOrWhiteSpace(newName.Text) &&
                !string.IsNullOrWhiteSpace(newEmail.Text) &&
                !string.IsNullOrWhiteSpace(newPass.Text))
            {
                // Pedir foto antes de proceder
                string photoPath = await TakePhotoAsync();

                if (string.IsNullOrEmpty(photoPath))
                {
                    statusMessage.Text = "Es necesario tomar una foto para completar el registro.";
                    return;
                }

                var user = new Usuario
                {
                    nombreUsuario = newName.Text,
                    email = newEmail.Text,
                    contraseña = newPass.Text,
                    imagenFoto = photoPath
                };

                App.UserRepository.AddUser(user);
                statusMessage.Text = App.UserRepository.statusMessage;
                await Shell.Current.GoToAsync("///MainPage");
            }
            else
            {
                statusMessage.Text = "Todos los campos son obligatorios.";
            }
        }

        private async Task<string> TakePhotoAsync()
        {
            try
            {
                var result = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions
                {
                    Title = "Tomar foto de perfil"
                });

                if (result != null)
                {
                    var newFile = Path.Combine(FileSystem.AppDataDirectory, $"{newName.Text}_profile.jpg");
                    using (var stream = await result.OpenReadAsync())
                    using (var newStream = File.OpenWrite(newFile))
                        await stream.CopyToAsync(newStream);

                    return newFile;
                }
            }
            catch (FeatureNotSupportedException)
            {
                statusMessage.Text = "La cámara no es soportada en este dispositivo.";
            }
            catch (PermissionException)
            {
                statusMessage.Text = "Permisos de cámara denegados.";
            }
            catch (Exception ex)
            {
                statusMessage.Text = $"Error al tomar foto: {ex.Message}";
            }

            return null;
        }
    }
}
