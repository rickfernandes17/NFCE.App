using Microsoft.EntityFrameworkCore;

namespace NFCEApp.Models
{
    public class NotaFiscal
    {
        public int id { get; set; }
        public string chave { get; set; }
        public DateTime dataCompra { get; set; }
        public bool Sincronizada { get; set; } = false;
        //public object produtos { get; set; }
        public int? idApi { get; set; } = 0;
    }
}
