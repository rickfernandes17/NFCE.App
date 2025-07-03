namespace NFCE.App.Views;

public partial class QRCodeScannerPageSelector : ContentPage
{
    public QRCodeScannerPageSelector()
    {
        InitializeComponent();
        NavigateToCorrectPage();
    }

    private async void NavigateToCorrectPage()
    {
        // Detecta a plataforma e navega para a página correta
#if WINDOWS
        await Shell.Current.GoToAsync("//QRCodeScannerPageWindows");
#else
        await Shell.Current.GoToAsync("//QRCodeScannerPage");
#endif
    }
}

