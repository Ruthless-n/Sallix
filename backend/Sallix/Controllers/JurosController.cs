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
        public IActionResult CalcularJuros([FromBody] RequestJuros request)
        {
            if (request.Valor <= 0 || request.DataVencimento == default)
                return BadRequest("Dados inválidos.");

            var resultado = _jurosService.CalcularValorJuros(request.Valor, request.DataVencimento);
            return Ok(resultado);
        }

    }
}
