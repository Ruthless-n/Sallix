namespace Sallix.Models
{
    public class ComissaoVenda
    {
        public string? Vendedor { get; set; }
        public decimal Valor { get; set; }
        public decimal ComissaoPercentual { get; set; }
        public decimal ValorComissao { get; set; }
    }
}
