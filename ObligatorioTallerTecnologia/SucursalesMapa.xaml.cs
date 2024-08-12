
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

       
        map.MoveToRegion(MapSpan.FromCenterAndRadius(location, Distance.FromMiles(2)));

        
        var userLocationCircle = new Circle
        {
            Center = new Location(location.Latitude, location.Longitude),
            Radius = new Distance(40), 
            StrokeColor = Colors.Blue, 
            StrokeWidth = 10, 
            FillColor = Colors.Red 
        };

    
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

    }

    private void OnAgregarSucursalClicked(object sender, EventArgs e)
    {
        
        if (string.IsNullOrWhiteSpace(entryNombre.Text) ||
            string.IsNullOrWhiteSpace(entryDireccion.Text) ||
            string.IsNullOrWhiteSpace(entryTelefono.Text))
        {
            DisplayAlert("Error", "Por favor, completa todos los campos obligatorios (Nombre, Direcci�n, Tel�fono).", "OK");
            return;
        }

        if (selectedPin == null)
        {
            DisplayAlert("Error", "Por favor, selecciona una ubicaci�n en el mapa.", "OK");
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
        CargarSucursales();

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
            CargarSucursales(); 
        }
    }

    private void CargarSucursales()
    {
        pickerSucursales.ItemsSource = null;
        var sucursales = App.UserRepository.GetAllSucursales();
        pickerSucursales.ItemsSource = sucursales;
        pickerSucursales.ItemDisplayBinding = new Binding("Nombre");

        map.Pins.Clear();

        
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
