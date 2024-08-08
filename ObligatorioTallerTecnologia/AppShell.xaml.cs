

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

            // Actualizar la visibilidad de los elementos
            loginShell.IsVisible = !isLoggedIn;
            registroShell.IsVisible = !isLoggedIn;
            perfilShell.IsVisible = isLoggedIn;

            // Redirigir a la página adecuada
           
        }
    }


}
