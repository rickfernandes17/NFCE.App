using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.ComponentModel;
using NFCEApp.Models;
using NFCEApp.DBContext;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using NFCE.App.Views;
using System.Text.Json;
using NFCEApp.Services;


namespace NFCE.App.ViewModels
{
    public class NotasViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<NotaFiscal> Notas { get; set; } = new();
        private NotaFiscal _nota = new NotaFiscal
        {
            dataCompra = DateTime.Now,
        };
        public NotaFiscal Nota
        {
            get => _nota;
            set
            {
                _nota = value;
                OnPropertyChanged(nameof(Nota));
            }
        }
        public ICommand AdicionarNotaCommand { get; }
        public ICommand EditarNotaCommand { get; }
        public ICommand SincronizarCommand { get; } // Adicionei o comando de sincronização
        public ICommand ExcluirNotaCommand { get; }
        private readonly AppDbContext _db = new AppDbContext();

        public NotasViewModel()
        {
            AdicionarNotaCommand = new Command(AdicionarNota);
            EditarNotaCommand = new Command<NotaFiscal>(EditarNota);
            ExcluirNotaCommand = new Command<NotaFiscal>(ExcluirNota);
            SincronizarCommand = new Command(async () => await SincronizarNotas());
        }

        public async Task CarregarNotas()
        {
            try
            {
                // Carregue do SQLite ou API local
                Notas.Clear();
                var notas = await _db.NotasFiscais.ToListAsync();
                foreach (var nota in notas)
                {
                    Notas.Add(nota);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", $"Falha ao carregar notas: {ex.Message}", "OK");
            }
            }

        private async void AdicionarNota()
        {
            string msg = "";
            try
            {
                if (Nota.id == 0)
                {
                    // Inserção
                    _db.NotasFiscais.Add(Nota);
                    msg = "Nota adicionada com sucesso!";
                }
                else
                {
                    // Edição
                    var local = _db.NotasFiscais.Local.FirstOrDefault(n => n.id == Nota.id);
                    if (local != null)
                    {
                        // Desanexa instância local
                        _db.Entry(local).State = EntityState.Detached;
                    }

                    // Atualiza
                    _db.Entry(Nota).State = EntityState.Modified;
                    msg = "Nota atualizada com sucesso!";
                }
                await _db.SaveChangesAsync();

                Notas.Add(Nota); // Atualiza a lista se necessário
                await CarregarNotas(); // Recarrega as notas após a adição/edição

                await Application.Current.MainPage.DisplayAlert("Sucesso", msg, "OK");

                await Shell.Current.GoToAsync(".."); // Volta para a tela anterior
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", $"Erro ao adicionar nota: {ex.Message}", "OK");
            }
        }
        private async void EditarNota(NotaFiscal nota)
        {
            if (nota == null)
                return;

            // Abre a página NotaPage passando a nota para edição
           await Shell.Current.GoToAsync(nameof(NotaPage), new Dictionary<string, object>{
                    { "NotaSelecionada", nota }
                });
                    }

        private async void ExcluirNota(NotaFiscal nota)
        {
            try
            {
                Notas.Remove(nota);
                // Remover do SQLite
                if (nota.Sincronizada && nota.idApi != 0)
                {
                    // Se já sincronizada, faça a exclusão na API
                    var apiService = new NotaFiscalApiService();
                    await apiService.DeleteNotaAsync(nota);
                }
                _db.NotasFiscais.Remove(nota);
                _db.SaveChangesAsync();
                await Application.Current.MainPage.DisplayAlert("Excluido",$"Excluido nota {nota.chave}", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", $"Erro ao excluir nota: {ex.Message}", "OK");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public async Task SincronizarNotas() {
            try
            {
                await SincronizarNotasEnvio();
                await SincronizarNotasRecebimento();
                await CarregarNotas();
                await Application.Current.MainPage.DisplayAlert("Sincronização","Atualizado", "OK");
            }
            catch (Exception ex) {
                await Application.Current.MainPage.DisplayAlert("Erro", $"Falha na sincronização: {ex.Message}", "OK");
            }
        }
        private async Task SincronizarNotasRecebimento()
        {
            try
            {
                var api = new NotaFiscalApiService();
                var notasDaApi = await api.GetNotasAsync();

                foreach (var nota in notasDaApi)
                {
                    // Verifica se já existe
                    bool existe = _db.NotasFiscais.Any(n => n.id == nota.id);
                    if (!existe)
                    {
                        nota.Sincronizada = true; // Marca como sincronizada
                        nota.idApi = nota.id;
                        _db.NotasFiscais.Add(nota); // Inclui os ProdutosNotas também se configurado o relacionamento
                    }
                }

                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Exiba o erro no Debug ou na interface
                await Application.Current.MainPage.DisplayAlert("Erro", $"Falha na sincronização: {ex.Message}", "OK");
            }
        }
        public async Task SincronizarNotasEnvio()
        {
            try
            {
                var notasNaoSincronizadas = await _db.NotasFiscais
                    .Where(n => !n.Sincronizada)
                    .ToListAsync();

                var apiService = new NotaFiscalApiService();

                foreach (var nota in notasNaoSincronizadas)
                {
                    // Cria uma cópia da nota com id = 0
                    var notaParaEnvio = new NotaFiscal
                    {
                        id = 0,
                        chave = nota.chave,
                        dataCompra = nota.dataCompra,
                        // demais campos
                    };

                    var notaCriada = await apiService.PostNotaAsync(notaParaEnvio);

                    if (notaCriada != null)
                    {
                        nota.Sincronizada = true;
                        nota.idApi = notaCriada.id;
                        _db.NotasFiscais.Update(nota);
                    }
                }

                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", $"Falha na sincronização: {ex.Message}", "OK");
            }
        }
    }
}
