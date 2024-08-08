using ObligatorioTallerTecnologia.Modelo;
namespace ObligatorioTallerTecnologia

{
    public partial class App : Application
    {
        public static UserRepository UserRepository { get; set; }
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
            MainPage = new AppShell();
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "users.db3");
            UserRepository = new UserRepository(dbPath);

        }
    }
}
