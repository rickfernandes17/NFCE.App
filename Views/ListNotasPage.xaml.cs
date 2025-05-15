using NFCE.App.ViewModels;

namespace NFCE.App.Views;

public partial class ListNotasPage : ContentPage
{
    private readonly NotasViewModel _viewModel;
    public ListNotasPage()
	{
		InitializeComponent();
        _viewModel = BindingContext as NotasViewModel;
    }

    private void Adicionar_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new Views.NotaPage());

        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }
    // Método chamado sempre que a página for exibida
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Carrega as notas do banco
        await _viewModel.CarregarNotas();
    }
}