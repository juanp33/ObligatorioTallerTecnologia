namespace ObligatorioTallerTecnologia
{
    using ObligatorioTallerTecnologia.Modelo;
    using Microsoft.Maui.Controls;

    public partial class MovieDetailsPage : ContentPage
    {
        public MovieDetailsPage(Movie movieDetails)
        {
            InitializeComponent();
            BindingContext = movieDetails;
        }
    }
}