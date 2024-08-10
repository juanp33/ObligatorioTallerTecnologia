
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Newtonsoft.Json.Linq;
using ObligatorioTallerTecnologia.Modelo;
using SQLite;

namespace ObligatorioTallerTecnologia;

public partial class SucursalesMapa : ContentPage
{
    private Pin selectedPin;
    private Pin userLocationPin;

    public SucursalesMapa()
    {
        InitializeComponent();
        CargarSucursales();
    }
   
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var geolocation = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(10));
        var location = await Geolocation.GetLocationAsync(geolocation);

        // Mover el mapa a la ubicación del usuario
        map.MoveToRegion(MapSpan.FromCenterAndRadius(location, Distance.FromMiles(2)));

        // Crear un círculo para la ubicación del usuario
        var userLocationCircle = new Circle
        {
            Center = new Location(location.Latitude, location.Longitude),
            Radius = new Distance(20), // Radio del círculo en metros
            StrokeColor = Colors.Blue, // Color del borde del círculo
            StrokeWidth = 2, // Grosor del borde
            FillColor = Colors.Red // Color de relleno con transparencia (opacidad)
        };

        // Añadir el círculo al mapa
        map.MapElements.Add(userLocationCircle);

        map.MapClicked += OnMapClicked;
    }

    private void OnMapClicked(object sender, MapClickedEventArgs e)
    {
        if (selectedPin != null)
        {
            map.Pins.Remove(selectedPin);
        }

        selectedPin = new Pin
        {
            Label = "Nueva Sucursal",
            Type = PinType.Place,
            Location = new Location(e.Location.Latitude, e.Location.Longitude)
        };

        map.Pins.Add(selectedPin);

        entryDireccion.Text = $"Lat: {selectedPin.Location.Latitude}, Long: {selectedPin.Location.Longitude}";
    }

    private void OnAgregarSucursalClicked(object sender, EventArgs e)
    {
        if (selectedPin == null)
        {
            DisplayAlert("Error", "Por favor, selecciona una ubicación en el mapa.", "OK");
            return;
        }

        var nuevaSucursal = new Sucursal
        {
            Nombre = entryNombre.Text,
            Direccion = entryDireccion.Text,
            Telefono = entryTelefono.Text,
            Latitud = selectedPin.Location.Latitude,
            Longitud = selectedPin.Location.Longitude
        };
        App.UserRepository.AddSucursal(nuevaSucursal);
        CargarSucursales(); // Actualizar la lista de sucursales

        // Limpiar después de agregar
        map.Pins.Remove(selectedPin);
        selectedPin = null;
        entryNombre.Text = string.Empty;
        entryDireccion.Text = string.Empty;
        entryTelefono.Text = string.Empty;
    }

    private void OnEliminarSucursalClicked(object sender, EventArgs e)
    {
        if (pickerSucursales.SelectedItem is Sucursal sucursalSeleccionada)
        {
            App.UserRepository.DeleteSucursal(sucursalSeleccionada);
            map.Pins.Remove(map.Pins.FirstOrDefault(pin => pin.Label == sucursalSeleccionada.Nombre));
            CargarSucursales(); // Actualizar la lista de sucursales
        }
    }

    private void CargarSucursales()
    {
        pickerSucursales.ItemsSource = null;
        var sucursales = App.UserRepository.GetAllSucursales();
        pickerSucursales.ItemsSource = sucursales;
        pickerSucursales.ItemDisplayBinding = new Binding("Nombre");

        map.Pins.Clear();

        // Volver a agregar el pin de la ubicación del usuario al limpiar los pins
        if (userLocationPin != null)
        {
            map.Pins.Add(userLocationPin);
        }
        try
        {
            foreach (var sucursal in sucursales)
            {
                var pin = new Pin
                {
                    Label = sucursal.Nombre,
                    Address = sucursal.Direccion,
                    Type = PinType.Place,
                    Location = new Location(sucursal.Latitud, sucursal.Longitud)
                };
                map.Pins.Add(pin);
            }
        }
        catch (Exception ex) { }
    }
}
