using ObligatorioTallerTecnologia.Modelo;
using System;
using System.Collections.Generic;

namespace ObligatorioTallerTecnologia;

public partial class RegistroUsuario : ContentPage
{
    public RegistroUsuario()
    {
        InitializeComponent();
    }

    public void OnNewButtonClicked(object sender, EventArgs args)
    {
        //statusMessage.Text = "";

        //if (!string.IsNullOrWhiteSpace(newName.Text))
        //{
        //    App.UserRepository.AddUser(newName.Text, newEmail.Text, newPass.Text);
        //    statusMessage.Text = App.UserRepository.StatusMessage;
        //}
        //else
        //{
        //    statusMessage.Text = "User name cannot be empty.";
        //}
    }

    public void OnGetButtonClicked(object sender, EventArgs args)
    {
        //statusMessage.Text = "";

        //List<Usuario> people = App.UserRepository.GetAll();
        //peopleList.ItemsSource = people;
    }
}