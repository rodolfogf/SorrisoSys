//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SorrisoSys.Data;
using SorrisoSys.Models;
using SorrisoSys.Repositories.Interfaces;
using System.Data.Common;

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Paciente>>> RetornarPacientes([FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            var pacientes = await _context.Pacientes
                .Skip(skip)
                .Take(take)
                .AsNoTracking() //melhora de performance para operações somente leitura
                .ToListAsync();

            return pacientes;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> RetornarPacientePorId(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);

            if (paciente == null) return NotFound();

            return Ok(paciente);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarPaciente(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null) return NotFound();

            _context.Remove(paciente);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbException)
            {
                throw;
            }
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePaciente(int id, [FromBody] Paciente pacienteAtualizar)
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
    }
}
