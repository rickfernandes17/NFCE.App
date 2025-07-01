using NFCEApp.Models;
using System.Collections.ObjectModel;

namespace NFCE.App.Views;

public partial class ProdutosDaNotaView : ContentView
{
	public ProdutosDaNotaView()
	{
		InitializeComponent();
	}
    public static readonly BindableProperty ProdutosProperty =
        BindableProperty.Create(nameof(Produtos), typeof(ObservableCollection<Produto>), typeof(ProdutosDaNotaView));

    public ObservableCollection<Produto> Produtos
    {
        get => (ObservableCollection<Produto>)GetValue(ProdutosProperty);
        set => SetValue(ProdutosProperty, value);
    }
}