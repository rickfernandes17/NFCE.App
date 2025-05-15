using NFCE.App.Views;

namespace NFCE.App
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(NotaPage), typeof(NotaPage));
        }
    }
}
