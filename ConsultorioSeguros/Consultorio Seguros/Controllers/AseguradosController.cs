using Consultorio_Seguros.Models.Model;
using Consultorio_Seguros.Service;
using Microsoft.AspNetCore.Mvc;

namespace Consultorio_Seguros.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AseguradosController : ControllerBase
    {
        private readonly AseguradosService _aseguradosService; 
        public AseguradosController(AseguradosService aseguradosService)
        {
            _aseguradosService = aseguradosService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsegurados()
        {
            var response = await _aseguradosService.GetAsegurados();
            if (response.error)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAseguroForId(int id)
        {
            var response = await _aseguradosService.GetAsegurado(id);
            if (response.error)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("consultar/{cedula}")]
        public async Task<IActionResult> GetSegurosForAsegurado(string cedula)
        {
            var response = await _aseguradosService.GetSegurosByCedula(cedula);
            return Ok(response);
        }

        [HttpGet("consultarById/{id}")]
        public async Task<IActionResult> GetSegurosForAseguradoId(int id)
        {
            var response = await _aseguradosService.GetSegurosById(id);
            return Ok(response);
        }

        [HttpPost("ingresar")]
        public async Task<IActionResult> AddAsegurados([FromBody] AseguradosModel asegurados)
        {
            var response = await _aseguradosService.AddAsegurado(asegurados);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] AseguradosModel asegurados)
        {
            var response = await _aseguradosService.UpdateAsegurado(id, asegurados);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeguro(int id)
        {
            var response = await _aseguradosService.DeleteAsegurado(id);
            return Ok(response);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            var response = await _aseguradosService.ProcessFileAsync(file);
            return Ok(response);
        }

        [HttpPost("AddSeguroToAsegurado/{aseguradoId}")]
        public async Task<IActionResult> AddSeguroToAsegurado(int aseguradoId, [FromBody] List<int> segurosIds)
        {
            var response = await _aseguradosService.AssignSegurosToAsegurado(aseguradoId, segurosIds);
            return Ok(response);
        }

        [HttpPost("DeleteSeguroToAsegurado/{aseguradoId}")]
        public async Task<IActionResult> DeleteSeguroToAsegurado(int aseguradoId, [FromBody] List<int> segurosIds)
        {
            var response = await _aseguradosService.DeleteSegurosToAsegurado(aseguradoId, segurosIds);
            return Ok(response);
        }

    }
}
