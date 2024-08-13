using ObligatorioTallerTecnologia.Modelo;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using System.Text.RegularExpressions;


namespace ObligatorioTallerTecnologia
{
    public partial class RegistroUsuario : ContentPage
    {
        public RegistroUsuario()
        {
            InitializeComponent();
        }
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

       
            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return regex.IsMatch(email);
        }
        public async void OnNewButtonClicked(object sender, EventArgs args)
        {
            statusMessage.Text = "";

            if (!string.IsNullOrWhiteSpace(newName.Text) &&
                !string.IsNullOrWhiteSpace(newEmail.Text) &&
                !string.IsNullOrWhiteSpace(newPass.Text) && !string.IsNullOrEmpty(newPhone.Text)) 
            {
                string photoPath = null;
                if (!double.TryParse(newPhone.Text, out double latitud))

                {
                    await DisplayAlert("Error", "Telefono debe ser un valor numerico.", "OK");
                    return;
                }
                if (!IsValidEmail(newEmail.Text))
                {
                    await DisplayAlert("Error", "El correo electrónico no es válido.", "OK");
                    return;
                }
#if ANDROID
        photoPath = await TakePhotoAsync();

        if (string.IsNullOrEmpty(photoPath))
        {
            statusMessage.Text = "Es necesario tomar una foto para completar el registro.";
            return;
        }
#endif

                var user = new Usuario
                {
                    nombreUsuario = newName.Text,
                    email = newEmail.Text,
                    contraseña = newPass.Text,
                    imagenFoto = photoPath,
                    telefono = newPhone.Text
                };

                App.UserRepository.AddUser(user);
                statusMessage.Text = App.UserRepository.statusMessage;
                await Navigation.PopAsync();
                await DisplayAlert("Registro exitoso", "Se registró correctamente", "OK");
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
