namespace Sallix.Models
{
    public class Juros
    {
        public decimal ValorInicial { get; set; }
        public DateTime DataVencimento { get; set; }
        public int DiasAtraso { get; set; }
        public decimal ValorJuros { get; set; }
        public decimal TotalPagar { get; set; }

    }
}
