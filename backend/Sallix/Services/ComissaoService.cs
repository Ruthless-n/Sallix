using Sallix.Models;
using System.Text.Json;

namespace Sallix.Services
{
    public class ComissaoService
    {
        private readonly string _jsonFilePath;

        public ComissaoService(string jsonFilePath)
        {
            _jsonFilePath = jsonFilePath;
        }

        private decimal CalcularPercentualComissao(decimal valor)
        {
            if (valor < 100)
                return 0;
            else if (valor < 500)
                return 1;
            else
                return 5;
        }

        public List<Venda> LerVendas()
        {
            try
            {
                string json = File.ReadAllText(_jsonFilePath);
                using JsonDocument doc = JsonDocument.Parse(json);
                
                var vendas = new List<Venda>();
                var vendasArray = doc.RootElement.GetProperty("vendas");

                foreach (var item in vendasArray.EnumerateArray())
                {
                    var venda = new Venda
                    {
                        Vendedor = item.GetProperty("vendedor").GetString(),
                        Valor = item.GetProperty("valor").GetDecimal()
                    };
                    vendas.Add(venda);
                }

                return vendas;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao ler arquivo de vendas: {ex.Message}");
            }
        }

        public List<ComissaoVenda> CalcularComissoesPorVenda()
        {
            var vendas = LerVendas();
            var comissoes = new List<ComissaoVenda>();

            foreach (var venda in vendas)
            {
                decimal percentual = CalcularPercentualComissao(venda.Valor ?? 0);
                decimal valorComissao = (venda.Valor ?? 0) * percentual / 100;

                comissoes.Add(new ComissaoVenda
                {
                    Vendedor = venda.Vendedor,
                    Valor = venda.Valor ?? 0,
                    ComissaoPercentual = percentual,
                    ValorComissao = valorComissao
                });
            }

            return comissoes;
        }

        public List<ResumoVendedor> CalcularResumoComissoes()
        {
            var comissoes = CalcularComissoesPorVenda();
            
            var resumo = comissoes
                .GroupBy(c => c.Vendedor)
                .Select(g => new ResumoVendedor
                {
                    Vendedor = g.Key,
                    TotalVendas = g.Sum(c => c.Valor),
                    TotalComissao = g.Sum(c => c.ValorComissao),
                    QuantidadeVendas = g.Count()
                })
                .OrderByDescending(r => r.TotalComissao)
                .ToList();

            return resumo;
        }
    }
}
