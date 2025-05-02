//using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SorrisoSys.Data;
using SorrisoSys.Data.DTOs;
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
        private readonly IMapper _mapper;

        public PacienteController(SorrisoSysContext context, IPacienteRepository pacienteRepository, IMapper mapper)
        {
            _context = context;
            _pacienteRepository = pacienteRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadPacienteDTO>>> RetornarPacientes([FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            try
            {
                var pacientes = await _pacienteRepository.GetAllPacienteAsync(skip, take);
                var pacientesDto = _mapper.Map<IEnumerable<ReadPacienteDTO>>(pacientes);
                return Ok(pacientesDto);
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
                var paciente = await _pacienteRepository.GetPacienteByIdAsync(id);
                var pacienteDto = _mapper.Map< ReadPacienteDTO>(paciente);

                return Ok(pacienteDto);
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
        public async Task<IActionResult> CadastrarPaciente([FromBody] CreatePacienteDTO pacienteDto)
        {
            Paciente result;

            try
            {
                var paciente = _mapper.Map<Paciente>(pacienteDto);
                result = await _pacienteRepository.AddPacienteAsync(paciente);
                var readDto = _mapper.Map<ReadPacienteDTO>(result);
                return CreatedAtAction(
                        nameof(RetornarPacientePorId)
                        , new { id = result.Id }
                        , readDto
                    );
            }
            catch (HttpRequestException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizaPaciente(int id, [FromBody] UpdatePacienteDTO pacienteDto)
        {
            Paciente result;

            try
            {
                var pacienteAtualizar = _mapper.Map<Paciente>(pacienteDto);
                result = await _pacienteRepository.UpdatePacienteAsync(id, pacienteAtualizar);
                var readDto = _mapper.Map<ReadPacienteDTO>(result);

                return Ok(readDto);

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
