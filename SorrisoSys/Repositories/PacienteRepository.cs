using Microsoft.EntityFrameworkCore;
using SorrisoSys.Data;
using SorrisoSys.Models;
using SorrisoSys.Repositories.Interfaces;
using FluentValidation.Results;

namespace SorrisoSys.Repositories
{
    public class Result<T>
    {
        public int HttpStatusCode { get; set; }
        public string Message { get; set; }

        public T Data { get; set; }
    }

    public class PacienteRepository : IPacienteRepository
    {
        private readonly SorrisoSysContext _context;

        public PacienteRepository(SorrisoSysContext context)
        {
            _context = context;
        }

        public ValidationResult ValidarDadosPaciente(Paciente paciente)
        {
            PacienteValidator validator = new();
            ValidationResult result = validator.Validate(paciente);

            if (!result.IsValid)
            {
                string erros = string.Join(Environment.NewLine, result.Errors.Select(e => e.ErrorMessage));

                throw new NotImplementedException(erros);
            }

            return result;
        }

        public async Task<bool> ValidarPacienteExistenteAsync(int id) // private
        {
            return await _context.Pacientes.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> ValidarCpfExistenteAsync(string? cpf) // private
        {
            if (string.IsNullOrEmpty(cpf)) return false;

            var paciente = await _context.Pacientes.FirstOrDefaultAsync(x => x.Cpf == cpf);
            return paciente != null;
        }
        public async Task<IEnumerable<Paciente>> GetAllPacienteAsync(int skip, int take)
        {
            if (skip < 0 || take <= 0)
            {
                throw new ArgumentException("Parâmetros de paginação inválidos");
            }

            try
            {
                return await _context.Pacientes
                    .Skip(skip)
                    .Take(take)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new HttpRequestException("Falha ao recuperar pacientes", ex);
            }
        }
        public async Task<Paciente?> GetPacienteByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID do paciente inválido");
            }

            var paciente = await _context.Pacientes.FirstOrDefaultAsync(x => x.Id == id);

            if (paciente == null)
            {
                throw new KeyNotFoundException("Paciente não encontrado");
            }

            return paciente;
        }
        public async Task<Paciente> AddPacienteAsync(Paciente paciente)
        {
            ValidarDadosPaciente(paciente);            
            
            if (await ValidarCpfExistenteAsync(paciente.Cpf))
            {
                throw new Exception("Já existe um paciente cadastrado com este CPF.");
            }

            var result  = await _context.Pacientes.AddAsync(paciente);
            
            if (await _context.SaveChangesAsync() <= 0) 
                throw new HttpRequestException("Não foi possível salvar o paciente");

            return result.Entity;
        }

        public async Task<Paciente> UpdatePacienteAsync(int id, Paciente paciente)
        {
            if (paciente == null)
            {
                throw new Exception("E necessário informar o ID e os dados do paciente a ser alterado!");
            }

            var pacienteOld = await GetPacienteByIdAsync(id);

            if (await ValidarCpfExistenteAsync(paciente.Cpf))
            {
                throw new Exception("Já existe um paciente cadastrado com este CPF.");
            }

            var result = await _context.Pacientes.AddAsync(paciente);

            if (await _context.SaveChangesAsync() <= 0)
                throw new HttpRequestException("Não foi possível salvar o paciente");

            return result.Entity;
        }

        public async Task DeletePaciente(int id)
        {
            var paciente = await GetPacienteByIdAsync(id);

            if (paciente != null)
            {
                _context.Remove(paciente);
                if (await _context.SaveChangesAsync() <= 0)
                {
                    throw new HttpRequestException("Falha ao excluir o paciente");
                }
            }
        }        
    }
}
