using SorrisoSys.Models;
using FluentValidation.Results;

namespace SorrisoSys.Repositories.Interfaces
{
    public interface IPacienteRepository
    {
        Task<bool> ValidarPacienteExistenteAsync(int id);
        Task<bool> ValidarCpfExistenteAsync(string? cpf);
        Task<Paciente?> GetPacienteByIdAsync(int id);
        Task<IEnumerable<Paciente>> GetAllPacienteAsync(int skip, int take);
        Task<Paciente> AddPacienteAsync(Paciente paciente);
        Task<Paciente> UpdatePacienteAsync(int id, Paciente paciente);
        Task DeletePaciente(int id);
        ValidationResult ValidarDadosPaciente(Paciente paciente);
    }
}