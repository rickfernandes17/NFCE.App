using Microsoft.EntityFrameworkCore;

namespace NFCEApp.Models
{
    public class ProdutoNota
    {
        
        public int Id { get; set; }
        public int NotaFiscalId { get; set; }
        public string Nome { get; set; }
        public decimal Quantidade { get; set; }
        public string Unidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public bool Marcado { get; set; } // ✔️ para checklist
    }
}
