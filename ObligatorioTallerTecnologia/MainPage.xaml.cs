using ObligatorioTallerTecnologia.Modelo;
using System.Linq.Expressions;

namespace ObligatorioTallerTecnologia
{
    public partial class MainPage : ContentPage
    {
      
       
        public MainPage()
        {
            InitializeComponent();


        }

        private async void Button_Popular(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("///popularmovies");
        }
        private async void Button_Movies(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("///allmovies");
        }
    }

}
