using Sallix.Models;
using System.Text.Json;

namespace Sallix.Services
{
    public class EstoqueService
    {
        private readonly string _jsonFilePath;
        private readonly string _historicoFilePath;
        private List<Produto>? _produtos;
        private List<MovimentacaoEstoque> _movimentacoes = new();
        private int _ultimoIdMovimentacao = 0;

        public EstoqueService(string jsonFilePath)
        {
            _jsonFilePath = jsonFilePath;
            _historicoFilePath = Path.Combine(Path.GetDirectoryName(jsonFilePath) ?? "", "historico_movimentacoes.json");
            CarregarProdutos();
            CarregarHistoricoMovimentacoes();
        }

        /// <summary>
        /// Carrega os produtos do arquivo JSON
        /// </summary>
        private void CarregarProdutos()
        {
            try
            {
                string json = File.ReadAllText(_jsonFilePath);
                using JsonDocument doc = JsonDocument.Parse(json);

                _produtos = new List<Produto>();
                var estoqueArray = doc.RootElement.GetProperty("estoque");

                foreach (var item in estoqueArray.EnumerateArray())
                {
                    var produto = new Produto
                    {
                        CodigoProduto = item.GetProperty("codigoProduto").GetInt32(),
                        DescricaoProduto = item.GetProperty("descricaoProduto").GetString(),
                        Estoque = item.GetProperty("estoque").GetInt32()
                    };
                    _produtos.Add(produto);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao ler arquivo de estoque: {ex.Message}");
            }
        }

        /// <summary>
        /// Carrega o histórico de movimentações do arquivo JSON
        /// </summary>
        private void CarregarHistoricoMovimentacoes()
        {
            try
            {
                if (File.Exists(_historicoFilePath))
                {
                    string json = File.ReadAllText(_historicoFilePath);
                    var opcoes = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    _movimentacoes = JsonSerializer.Deserialize<List<MovimentacaoEstoque>>(json, opcoes) ?? new List<MovimentacaoEstoque>();
                    
                    if (_movimentacoes.Any())
                    {
                        _ultimoIdMovimentacao = _movimentacoes.Max(m => m.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Aviso ao carregar histórico: {ex.Message}");
                _movimentacoes = new List<MovimentacaoEstoque>();
            }
        }

        /// <summary>
        /// Salva apenas o histórico de movimentações
        /// </summary>
        private void SalvarHistoricoMovimentacoes()
        {
            try
            {
                var opcoes = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(_movimentacoes, opcoes);
                File.WriteAllText(_historicoFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar histórico: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtém um produto pelo código
        /// </summary>
        public Produto? ObterProduto(int codigoProduto)
        {
            return _produtos?.FirstOrDefault(p => p.CodigoProduto == codigoProduto);
        }

        /// <summary>
        /// Obtém todos os produtos
        /// </summary>
        public List<Produto> ObterTodosProdutos()
        {
            return _produtos ?? new List<Produto>();
        }

        /// <summary>
        /// Registra uma movimentação de estoque (entrada ou saída)
        /// </summary>
        public RespostaMovimentacao RegistrarMovimentacao(
            int codigoProduto,
            string tipoMovimentacao,
            int quantidade,
            string descricao)
        {
            var resposta = new RespostaMovimentacao();

            try
            {
                // Validações
                if (quantidade <= 0)
                {
                    resposta.Sucesso = false;
                    resposta.Mensagem = "A quantidade deve ser maior que zero.";
                    return resposta;
                }

                if (string.IsNullOrWhiteSpace(descricao))
                {
                    resposta.Sucesso = false;
                    resposta.Mensagem = "A descrição é obrigatória.";
                    return resposta;
                }

                if (tipoMovimentacao != "Entrada" && tipoMovimentacao != "Saída")
                {
                    resposta.Sucesso = false;
                    resposta.Mensagem = "Tipo de movimentação inválido. Use 'Entrada' ou 'Saída'.";
                    return resposta;
                }

                var produto = ObterProduto(codigoProduto);
                if (produto == null)
                {
                    resposta.Sucesso = false;
                    resposta.Mensagem = $"Produto com código {codigoProduto} não encontrado.";
                    return resposta;
                }

                int estoqueAnterior = produto.Estoque;
                int novoEstoque = estoqueAnterior;

                // Calcula novo estoque
                if (tipoMovimentacao == "Entrada")
                {
                    novoEstoque += quantidade;
                }
                else if (tipoMovimentacao == "Saída")
                {
                    if (estoqueAnterior < quantidade)
                    {
                        resposta.Sucesso = false;
                        resposta.Mensagem = $"Estoque insuficiente. Disponível: {estoqueAnterior}, Solicitado: {quantidade}";
                        resposta.EstoqueAtual = estoqueAnterior;
                        return resposta;
                    }
                    novoEstoque -= quantidade;
                }

                // Atualiza o estoque do produto (apenas em memória)
                produto.Estoque = novoEstoque;

                // Registra a movimentação
                _ultimoIdMovimentacao++;
                var movimentacao = new MovimentacaoEstoque
                {
                    Id = _ultimoIdMovimentacao,
                    CodigoProduto = codigoProduto,
                    DescricaoProduto = produto.DescricaoProduto,
                    TipoMovimentacao = tipoMovimentacao,
                    Descricao = descricao,
                    Quantidade = quantidade,
                    DataMovimentacao = DateTime.Now,
                    EstoqueAnterior = estoqueAnterior,
                    EstoqueAtual = novoEstoque
                };

                _movimentacoes.Add(movimentacao);

                // SALVAR APENAS O HISTÓRICO
                SalvarHistoricoMovimentacoes();

                resposta.Sucesso = true;
                resposta.Mensagem = "Movimentação registrada com sucesso.";
                resposta.Movimentacao = movimentacao;
                resposta.EstoqueAtual = novoEstoque;

                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Sucesso = false;
                resposta.Mensagem = $"Erro ao registrar movimentação: {ex.Message}";
                return resposta;
            }
        }

        /// <summary>
        /// Obtém o histórico de movimentações
        /// </summary>
        public List<MovimentacaoEstoque> ObterHistoricoMovimentacoes()
        {
            return _movimentacoes.OrderByDescending(m => m.DataMovimentacao).ToList();
        }

        /// <summary>
        /// Obtém o histórico de movimentações de um produto específico
        /// </summary>
        public List<MovimentacaoEstoque> ObterHistoricoMovimentacoesPorProduto(int codigoProduto)
        {
            return _movimentacoes
                .Where(m => m.CodigoProduto == codigoProduto)
                .OrderByDescending(m => m.DataMovimentacao)
                .ToList();
        }
    }
}
