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


namespace NFCE.App.ViewModels
{
    public class NotasViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<NotaFiscal> Notas { get; set; } = new();
        public ICommand AdicionarNotaCommand { get; }
        public ICommand ExcluirNotaCommand { get; }
        private readonly AppDbContext _db = new AppDbContext();

        public NotasViewModel()
        {
            AdicionarNotaCommand = new Command(AdicionarNota);
            ExcluirNotaCommand = new Command<NotaFiscal>(ExcluirNota);
            CarregarNotas();
        }

        private void CarregarNotas()
        {
            // Carregue do SQLite ou API local
            Notas.Clear();
            var notas = _db.NotasFiscais.ToList();
            foreach (var nota in notas)
            {
                Notas.Add(nota);
            }
        }

        private void AdicionarNota()
        {
            // Lógica para nova nota (abrir tela, ou inserir diretamente)
        }

        private void ExcluirNota(NotaFiscal nota)
        {
            Notas.Remove(nota);
            // Remover do SQLite
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
