using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
namespace ObligatorioTallerTecnologia;

public partial class SucursalesMapa : ContentPage
{
    public SucursalesMapa()
    {
        InitializeComponent();


    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var geolocation = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(10));
        var location = await Geolocation.GetLocationAsync(geolocation);
        map.MoveToRegion(MapSpan.FromCenterAndRadius(location, Distance.FromMiles(2)));

        var pin = new Pin
        {
            Label = "Sucursal Punta",
            Address = " Punta Shopping",
            Type = PinType.Place,
            Location = new Location(-34.941027, -54.934873)
        };
        var pin1 = new Pin
        {
            Label = "Sucursal Atlantico",
            Address = "Atlantico Shopping",
            Type = PinType.Place,
            Location = new Location(-34.916300, -54.961057)
        };
        map.Pins.Add(pin);
        map.Pins.Add(pin1);
    }
}