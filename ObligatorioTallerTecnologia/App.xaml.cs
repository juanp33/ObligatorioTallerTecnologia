using ObligatorioTallerTecnologia.Modelo;

namespace ObligatorioTallerTecnologia;

    public partial class App : Application
    {
        public static UserRepository UserRepository { get; set; }
        public App(UserRepository _UserRepository)
        {
            InitializeComponent();

            MainPage = new AppShell();
        //MainPage = new NavigationPage(new MoviesPage());
        UserRepository = _UserRepository;
        }
    }

