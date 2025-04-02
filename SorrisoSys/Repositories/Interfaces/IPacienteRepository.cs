using SorrisoSys.Models;

namespace SorrisoSys.Repositories.Interfaces
{
    public interface IPacienteRepository
    {
        Task<bool> ValidarPacienteExistenteAsync(int id);
        Task<bool> ValidarCpfExistenteAsync(string? cpf);
        Task<Paciente> AddPacienteAsync(Paciente paciente);
        Task<Paciente> UpdatePacienteAsync(int id, Paciente paciente);
    }
}