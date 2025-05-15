using Microsoft.EntityFrameworkCore;
using NFCEApp.Services;
using System.ComponentModel;
using System.Linq;
using System.Collections.ObjectModel;
using NFCEApp.DBContext;
using NFCEApp.Models;
using System.Text.Json;

namespace NFCE.App
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        private readonly AppDbContext _db;
        private readonly NotaFiscalApiService _apiService;

        public MainPage(AppDbContext db,NotaFiscalApiService apiService)
        {
            InitializeComponent();
            _db = db;
            _apiService = apiService;
  
            
        }
        protected override async void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                //await SincronizarNotas(); // Agora o método será executado corretamente
                await CarregarNotas(); // Carrega as notas após a sincronização
            }
            catch (Exception ex) { 
                await DisplayAlert("Erro", $"Falha ao carregar notas: {ex.Message}", "OK"); // Exibe erro
            }
        }
        private async Task SincronizarNotas()
        {
            try
            {
                var notasDaApi = await _apiService.GetNotasAsync();

                foreach (var nota in notasDaApi)
                {
                    // Verifica se já existe
                    bool existe = _db.NotasFiscais.Any(n => n.id == nota.id);
                    if (!existe)
                    {
                        nota.Sincronizada = true; // Marca como sincronizada
                        _db.NotasFiscais.Add(nota); // Inclui os ProdutosNotas também se configurado o relacionamento
                    }
                }

                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Exiba o erro no Debug ou na interface
                await DisplayAlert("Erro", $"Falha na sincronização: {ex.Message}", "OK");
            }
        }

        private async Task CarregarNotas()
        {
            var notas = await _db.NotasFiscais.ToListAsync();
            Notas.ItemsSource = notas;
        }
        private async void OnSincronizarClicked(object sender, EventArgs e)
        {
            try
            {
                // Pode usar indicador de carregamento se quiser
                await SincronizarNotas();
                await CarregarNotas(); // Atualiza a lista após a sincronização
                await DisplayAlert("Sucesso", "Notas sincronizadas com sucesso!", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Falha na sincronização: {ex.Message}", "OK");
            }
        }
        private async void OnExcluirClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is NotaFiscal nota)
            {
                bool confirmar = await DisplayAlert("Confirmação", $"Excluir nota {nota.chave}?", "Sim", "Não");
                if (confirmar)
                {
                    //((ObservableCollection<NotaFiscal>)Notas.ItemsSource).Remove(nota); // remove da lista visível
                    _db.NotasFiscais.Remove(nota); // remove do banco de dados
                    _db.SaveChanges(); // salva as alterações
                    CarregarNotas(); // atualiza a lista
                }
            }
        }
    }

}
