
namespace ObligatorioTallerTecnologia
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MoviesPage());
            MainPage = new AppShell();
        }
    }
}
