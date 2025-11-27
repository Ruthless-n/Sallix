using Microsoft.AspNetCore.Mvc;
using Sallix.Models;
using Sallix.Services;

namespace Sallix.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstoqueController : ControllerBase
    {
        private readonly EstoqueService _estoqueService;

        public EstoqueController(IWebHostEnvironment env)
        {
            string jsonPath = Path.Combine(env.ContentRootPath, "Json", "estoque.json");
            _estoqueService = new EstoqueService(jsonPath);
        }

        /// <summary>
        /// Obtém todos os produtos com seu estoque atual
        /// </summary>
        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Produto>> GetProdutos()
        {
            try
            {
                var produtos = _estoqueService.ObterTodosProdutos();
                return Ok(produtos);
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        /// <summary>
        /// Obtém um produto específico pelo código
        /// </summary>
        [HttpGet("produtos/{codigoProduto}")]
        public ActionResult<Produto> GetProduto(int codigoProduto)
        {
            try
            {
                var produto = _estoqueService.ObterProduto(codigoProduto);
                if (produto == null)
                {
                    return NotFound(new { erro = $"Produto com código {codigoProduto} não encontrado." });
                }
                return Ok(produto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        /// <summary>
        /// Registra uma movimentação de estoque (entrada ou saída)
        /// </summary>
        [HttpPost("movimentar")]
        public ActionResult<RespostaMovimentacao> RegistrarMovimentacao(
            [FromBody] RequisicaoMovimentacao requisicao)
        {
            try
            {
                var resposta = _estoqueService.RegistrarMovimentacao(
                    requisicao.CodigoProduto,
                    requisicao.TipoMovimentacao ?? "Entrada",
                    requisicao.Quantidade,
                    requisicao.Descricao ?? "");

                if (!resposta.Sucesso)
                {
                    return BadRequest(resposta);
                }

                var historico = _estoqueService.ObterHistoricoMovimentacoes();

                return Ok(new {
                    resposta.Sucesso,
                    resposta.Mensagem,
                    resposta.Movimentacao,
                    resposta.EstoqueAtual,
                    HistoricoAtualizado = historico
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        /// <summary>
        /// Obtém o histórico de todas as movimentações
        /// </summary>
        [HttpGet("historico")]
        public ActionResult<IEnumerable<MovimentacaoEstoque>> GetHistorico()
        {
            try
            {
                var historico = _estoqueService.ObterHistoricoMovimentacoes();
                return Ok(historico);
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        /// <summary>
        /// Obtém o histórico de movimentações de um produto específico
        /// </summary>
        [HttpGet("historico/{codigoProduto}")]
        public ActionResult<IEnumerable<MovimentacaoEstoque>> GetHistoricoPorProduto(int codigoProduto)
        {
            try
            {
                var historico = _estoqueService.ObterHistoricoMovimentacoesPorProduto(codigoProduto);
                return Ok(historico);
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }
    }

    /// <summary>
    /// Modelo de requisição para movimentação de estoque
    /// </summary>
    public class RequisicaoMovimentacao
    {
        public int CodigoProduto { get; set; }
        public string? TipoMovimentacao { get; set; } // "Entrada" ou "Saída"
        public int Quantidade { get; set; }
        public string? Descricao { get; set; }
    }
}
