using Microsoft.AspNetCore.Mvc;
using Sallix.Models;
using Sallix.Services;

namespace Sallix.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComissaoController : ControllerBase
    {
        private readonly ComissaoService _comissaoService;

        public ComissaoController(IWebHostEnvironment env)
        {
            string jsonPath = Path.Combine(env.ContentRootPath, "Json", "vendas.json");
            _comissaoService = new ComissaoService(jsonPath);
        }

        /// <summary>
        /// Retorna o resumo de comissões por vendedor
        /// </summary>
        [HttpGet("resumo")]
        public ActionResult<IEnumerable<ResumoVendedor>> GetResumoComissoes()
        {
            try
            {
                var resumo = _comissaoService.CalcularResumoComissoes();
                return Ok(resumo);
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }
    }
}
