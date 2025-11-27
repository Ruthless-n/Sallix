namespace Sallix.Models
{
    public class MovimentacaoEstoque
    {
        public int Id { get; set; }
        public int CodigoProduto { get; set; }
        public string? DescricaoProduto { get; set; }
        public string? TipoMovimentacao { get; set; } // "Entrada" ou "Saída"
        public string? Descricao { get; set; }
        public int Quantidade { get; set; }
        public DateTime DataMovimentacao { get; set; }
        public int EstoqueAnterior { get; set; }
        public int EstoqueAtual { get; set; }
    }
}
