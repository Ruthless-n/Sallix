using Microsoft.AspNetCore.Mvc;
using Sallix.Models;

namespace Sallix.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JurosController : ControllerBase
    {
        private readonly Services.JurosService _jurosService;

        public JurosController(Services.JurosService jurosService)
        {
            _jurosService = jurosService;
        }

        [HttpPost("calcular-juros")]
        public IActionResult CalcularJuros([FromQuery] decimal valorInicial, [FromQuery] DateTime dataVencimento)
        {
            if (valorInicial <= 0 || dataVencimento == default)
            {
                return BadRequest("Dados inválidos.");
            }

            var resultado = _jurosService.CalcularValorJuros(valorInicial, dataVencimento);
            return Ok(resultado);
        }
    }
}
