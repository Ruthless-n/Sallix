namespace Sallix.Services
{
    public class JurosService
    {
        public decimal CalcularValorJuros(decimal valorInicial, DateTime dataVencimento)
        {
            DateTime dataHoje = DateTime.Now;

            if (dataHoje <= dataVencimento)
            {
                return valorInicial;
            }

            int diasAtraso = (dataHoje - dataVencimento).Days;
            decimal taxaJurosDiaria = 0.025m;
            decimal jurosPercentTotal = diasAtraso * taxaJurosDiaria;
            decimal valorComJuros = valorInicial + (valorInicial * jurosPercentTotal);

            return Math.Round(valorComJuros, 2);
        }
    }
}