using NFCE.App.ViewModels;
using NFCEApp.Models;

namespace NFCE.App.Views;

public partial class NotaPage : ContentPage, IQueryAttributable
{
	public NotaPage()
	{
		InitializeComponent();
        BindingContext = new NotasViewModel();
    }
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("NotaSelecionada", out var nota))
        {
            var viewModel = BindingContext as NotasViewModel;
            viewModel.Nota = nota as NotaFiscal;
        }
    }
}