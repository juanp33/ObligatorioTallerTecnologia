

namespace ObligatorioTallerTecnologia
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            UpdateShell();
        }

        public void UpdateShell()
        {
            bool isLoggedIn = Preferences.Get("IsLoggedIn", false);

           
            loginShell.IsVisible = !isLoggedIn;
            registroShell.IsVisible = !isLoggedIn;
            favoritosShell.IsVisible = isLoggedIn;
            perfilShell.IsVisible = isLoggedIn;

         
        }
    }


}
