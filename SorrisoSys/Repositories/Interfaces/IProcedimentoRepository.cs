using SorrisoSys.Models;

namespace SorrisoSys.Repositories.Interfaces
{
    public interface IProcedimentoRepository
    {
        Task<bool> ValidarProcedimentoExistenteAsync(Procedimento procedimento);
    }
}
