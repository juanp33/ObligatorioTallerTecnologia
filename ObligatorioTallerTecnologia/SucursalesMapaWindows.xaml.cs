using ObligatorioTallerTecnologia.Modelo;
using System;
using Microsoft.Maui.Controls;

namespace ObligatorioTallerTecnologia;

public partial class SucursalesWindows : ContentPage
{
    public SucursalesWindows()
    {
        InitializeComponent();
        CargarSucursales();
    }

    private void OnAgregarSucursalClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(entryNombre.Text) ||
            string.IsNullOrWhiteSpace(entryDireccion.Text) ||
            string.IsNullOrWhiteSpace(entryTelefono.Text) ||
            string.IsNullOrWhiteSpace(entryLatitud.Text) ||
            string.IsNullOrWhiteSpace(entryLongitud.Text))
        {
            DisplayAlert("Error", "Por favor, completa todos los campos obligatorios (Nombre, Dirección, Teléfono, Latitud, Longitud).", "OK");
            return;
        }

       
        if (!double.TryParse(entryLatitud.Text, out double latitud) ||
            !double.TryParse(entryLongitud.Text, out double longitud))
        {
            DisplayAlert("Error", "Latitud y Longitud deben ser valores numéricos.", "OK");
            return;
        }

        var nuevaSucursal = new Sucursal
        {
            Nombre = entryNombre.Text,
            Direccion = entryDireccion.Text,
            Telefono = entryTelefono.Text,
            Latitud = latitud,
            Longitud = longitud
        };

        App.UserRepository.AddSucursal(nuevaSucursal);
        CargarSucursales();

       
        entryNombre.Text = string.Empty;
        entryDireccion.Text = string.Empty;
        entryTelefono.Text = string.Empty;
        entryLatitud.Text = string.Empty;
        entryLongitud.Text = string.Empty;
    }

    private void OnEliminarSucursalClicked(object sender, EventArgs e)
    {
        if (pickerSucursales.SelectedItem is Sucursal sucursalSeleccionada)
        {
            App.UserRepository.DeleteSucursal(sucursalSeleccionada);
            CargarSucursales();
        }
    }

    private void CargarSucursales()
    {
        pickerSucursales.ItemsSource = null;
        var sucursales = App.UserRepository.GetAllSucursales();
        pickerSucursales.ItemsSource = sucursales;
        pickerSucursales.ItemDisplayBinding = new Binding("Nombre");
    }
}
