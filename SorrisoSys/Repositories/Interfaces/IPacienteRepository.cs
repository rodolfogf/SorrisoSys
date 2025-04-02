namespace SorrisoSys.Repositories.Interfaces
{
    public interface IPacienteRepository
    {
        Task<bool> ValidarPacienteExistenteAsync(int id);
        Task<bool> ValidarCpfExistenteAsync(string? cpf);
    }
}