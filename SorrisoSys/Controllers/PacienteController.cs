//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SorrisoSys.Data;
using SorrisoSys.Models;
using SorrisoSys.Repositories.Interfaces;

namespace SorrisoSys.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PacienteController : Controller
    {
        private readonly SorrisoSysContext _context;
        private readonly IPacienteRepository _pacienteRepository;

        public PacienteController(SorrisoSysContext context, IPacienteRepository pacienteRepository)
        {
            _context = context;
            _pacienteRepository = pacienteRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Paciente>>> RetornarPacientes([FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            try
            {
                var result = await _pacienteRepository.GetAllPacienteAsync(skip, take);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno ao listar pacientes: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> RetornarPacientePorId(int id)
        {
            try
            {
                var result = await _pacienteRepository.GetPacienteByIdAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno ao buscar paciente: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarPaciente([FromBody] Paciente paciente)
        {
            Paciente result;

            try
            {
                result = await _pacienteRepository.AddPacienteAsync(paciente);
            }
            catch (HttpRequestException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            if (result == null)
                return BadRequest(result);

            return Ok(result);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizaPaciente(int id, [FromBody] Paciente pacienteAtualizar)
        {
            Paciente result;

            try
            {
                result = await _pacienteRepository.UpdatePacienteAsync(id, pacienteAtualizar);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return Problem(ex.Message);
            }

            return Ok(result);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarPaciente(int id)
        {
            try
            {
                await _pacienteRepository.DeletePaciente(id);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(HttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return Problem(ex.Message);
            }

            return NoContent();
        }
    }
}
