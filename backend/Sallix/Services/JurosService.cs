using System;
using System.Drawing;
using Sallix.Models;

namespace Sallix.Services
{
    public class JurosService
    {
        public Juros CalcularValorJuros(decimal valorInicial, DateTime dataVencimento)
        {
            DateTime dataHoje = DateTime.Today;
            int diasAtraso = (dataHoje - dataVencimento).Days;

            decimal jurosAoDia = 0.025m;
            decimal valorJuros = valorInicial * jurosAoDia * diasAtraso; 
            decimal total = valorInicial + valorJuros;

            return new Juros
            {
                ValorInicial = valorInicial, 
                DataVencimento = dataVencimento, 
                DiasAtraso = diasAtraso,
                ValorJuros = valorJuros,
                TotalPagar = total
            };
        }
    }
}