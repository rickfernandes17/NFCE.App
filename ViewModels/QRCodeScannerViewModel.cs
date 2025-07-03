using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NFCE.App.ViewModels
{
    public class QRCodeScannerViewModel : INotifyPropertyChanged
    {
        private string _qrCodeUrl = string.Empty;
        private bool _isScanning = true;
        private DateTime? _lastScanTime;

        public string QRCodeUrl
        {
            get => _qrCodeUrl;
            set
            {
                _qrCodeUrl = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasResult));
            }
        }

        public bool IsScanning
        {
            get => _isScanning;
            set
            {
                _isScanning = value;
                OnPropertyChanged();
            }
        }

        public DateTime? LastScanTime
        {
            get => _lastScanTime;
            set
            {
                _lastScanTime = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(LastScanTimeFormatted));
            }
        }

        public bool HasResult => !string.IsNullOrEmpty(QRCodeUrl);

        public string LastScanTimeFormatted => LastScanTime?.ToString("dd/MM/yyyy HH:mm:ss") ?? "Nunca";

        public void SetQRCodeResult(string url)
        {
            QRCodeUrl = url;
            LastScanTime = DateTime.Now;
        }

        public void ClearResult()
        {
            QRCodeUrl = string.Empty;
            LastScanTime = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

