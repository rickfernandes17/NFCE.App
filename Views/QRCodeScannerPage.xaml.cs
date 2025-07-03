using System.ComponentModel;
using ZXing.Net.Maui;

namespace NFCE.App.Views;

public partial class QRCodeScannerPage : ContentPage, INotifyPropertyChanged
{
    private string _qrCodeResult = string.Empty;
    private bool _isResultVisible = false;
    private bool _isDetecting = true;

    public string QRCodeResult
    {
        get => _qrCodeResult;
        set
        {
            _qrCodeResult = value;
            OnPropertyChanged();
        }
    }

    public bool IsResultVisible
    {
        get => _isResultVisible;
        set
        {
            _isResultVisible = value;
            OnPropertyChanged();
        }
    }

    public bool IsDetecting
    {
        get => _isDetecting;
        set
        {
            _isDetecting = value;
            OnPropertyChanged();
        }
    }

    public QRCodeScannerPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    private void OnBarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        var first = e.Results?.FirstOrDefault();
        if (first is not null)
        {
            // Armazena o resultado na variável
            QRCodeResult = first.Value;
            IsResultVisible = true;

            // Para o scanner automaticamente após detectar um QR code
            CameraView.IsDetecting = false;
            IsDetecting = false;

            // Atualiza o texto do botão
            StartStopButton.Text = "Iniciar Scanner";
            StartStopButton.BackgroundColor = Colors.Green;

            // Opcional: Exibir um alerta com o resultado
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await DisplayAlert("QR Code Detectado", $"URL: {QRCodeResult}", "OK");
            });
        }
    }

    private void OnStartStopClicked(object sender, EventArgs e)
    {
        if (IsDetecting)
        {
            // Parar o scanner
            CameraView.IsDetecting = false;
            IsDetecting = false;
            StartStopButton.Text = "Iniciar Scanner";
            StartStopButton.BackgroundColor = Colors.Green;
        }
        else
        {
            // Iniciar o scanner
            CameraView.IsDetecting = true;
            IsDetecting = true;
            StartStopButton.Text = "Parar Scanner";
            StartStopButton.BackgroundColor = Colors.Red;
        }
    }

    private void OnClearClicked(object sender, EventArgs e)
    {
        QRCodeResult = string.Empty;
        IsResultVisible = false;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

