

namespace ObligatorioTallerTecnologia
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            UpdateShell();
#if WINDOWS
            
            this.Items.Remove(sucursalesShell);
#endif
#if ANDROID    
            this.Items.Add(sucursalesWindowsShell);

#endif
        }

        public void UpdateShell()
        {
            bool isLoggedIn = Preferences.Get("IsLoggedIn", false);

           
            loginShell.IsVisible = !isLoggedIn;
            registroShell.IsVisible = !isLoggedIn;
            favoritosShell.IsVisible = isLoggedIn;
            perfilShell.IsVisible = isLoggedIn;
            sucursalesShell.IsVisible = isLoggedIn;
            sucursalesWindowsShell.IsVisible = isLoggedIn;

        }
    }


}
