using Consultorio_Seguros.Models.BD;
using Consultorio_Seguros.Models.Model;
using Consultorio_Seguros.Service;
using Microsoft.AspNetCore.Mvc;

namespace Consultorio_Seguros.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SegurosController : ControllerBase
    {
        private readonly SegurosService _segurosService;
        public SegurosController(SegurosService segurosService)
        {
            _segurosService = segurosService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSeguros()
        {
            var response = await _segurosService.GetSeguros();
            if (response.error)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("ingresar")]
        public async Task<IActionResult> PostSeguro([FromBody] SegurosModel seguro)
        {
            var response = await _segurosService.AddSeguro(seguro);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSeguroForId(int id)
        {
            var response = await _segurosService.GetSeguro(id);
            if (response.error)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("consultar/{codigo}")]
        public async Task<IActionResult> GetAseguradosForSeguro(string codigo)
        {
            var response = await _segurosService.GetAseguradosByCodigoSeguro(codigo);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] SegurosModel seguro)
        {
            var response = await _segurosService.UpdateSeguro(id, seguro);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeguro(int id)
        {
            var response = await _segurosService.DeleteSeguro(id);
            if (response.error)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
