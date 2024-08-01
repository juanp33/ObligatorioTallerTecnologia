
using ObligatorioTallerTecnologia.Modelo;

namespace ObligatorioTallerTecnologia;

public partial class RegistroUsuario : ContentPage
{
	public RegistroUsuario()
	{
		InitializeComponent();
	}

    public void OnNewButtonClicked(object sender, EventArgs args)
    {
        statusMessage.Text = "";

        App.UserRepository.AddUser(newName.Text,);
        statusMessage.Text = App.UserRepository.StatusMessage;
    }

    public void OnGetButtonClicked(object sender, EventArgs args)
    {
        statusMessage.Text = "";

        List<Usuario> people = App.UserRepository.GetAll();
        peopleList.ItemsSource = people;
    }

}