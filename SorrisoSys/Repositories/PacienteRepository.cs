using Microsoft.EntityFrameworkCore;
using SorrisoSys.Data;
using SorrisoSys.Models;
using SorrisoSys.Repositories.Interfaces;

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

        public async Task<Paciente> GetPacienteById(int id)
        {
            return await _context.Pacientes.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Paciente> AddPacienteAsync(Paciente paciente)
        {
            if (await ValidarCpfExistenteAsync(paciente.Cpf))
            {
                /*return new Result
                {
                    HttpStatusCode = 404,
                    Message = "Já existe um paciente cadastrado com este CPF."
                };*/

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

            var pacienteOld = await GetPacienteById(id);

            if(pacienteOld == null)
            {
                throw new KeyNotFoundException("Paciente não encontrado");
            }
            
            if (await ValidarCpfExistenteAsync(paciente.Cpf))
            {
                throw new Exception("Já existe um paciente cadastrado com este CPF.");
            }

            var result = await _context.Pacientes.AddAsync(paciente);

            if (await _context.SaveChangesAsync() <= 0)
                throw new HttpRequestException("Não foi possível salvar o paciente");

            return result.Entity;
        }
    }
}
