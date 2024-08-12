

namespace ObligatorioTallerTecnologia
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            UpdateShell();
#if WINDOWS
            // Oculta la pestaña de "Sucursales" en Windows
            this.Items.Remove(sucursalesShell);
#endif
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
