namespace Sallix.Models
{
    public class RespostaMovimentacao
    {
        public bool Sucesso { get; set; }
        public string? Mensagem { get; set; }
        public MovimentacaoEstoque? Movimentacao { get; set; }
        public int EstoqueAtual { get; set; }
    }
}
