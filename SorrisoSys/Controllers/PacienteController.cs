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
    public class PacienteController : ControllerBase
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
            if (await _pacienteRepository.ValidarCpfExistenteAsync(paciente.Cpf))
            {
                return Conflict("Já existe um paciente cadastrado com este CPF.");
            }
            try
            {
                await _context.Pacientes.AddAsync(paciente);
                await _context.SaveChangesAsync();
            }
            catch(DbException)
            {
                throw;
            }

            return CreatedAtAction(
                    nameof(RetornarPacientePorId)
                    ,new { id = paciente.Id }
                    ,paciente);
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
            catch(DbException) 
            {
                throw;
            }
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarPaciente(int id, [FromBody] Paciente pacienteAtualizar) 
        {
            if(id != pacienteAtualizar.Id)
            {
                return BadRequest("Id do paciente incompatível");
            }

            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null) return NotFound();

            _context.Entry(paciente).CurrentValues.SetValues(pacienteAtualizar);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) 
            {
                if (await _pacienteRepository.ValidarPacienteExistenteAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
    }
}
