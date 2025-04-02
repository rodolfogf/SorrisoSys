using Microsoft.EntityFrameworkCore;
using SorrisoSys.Data;
using SorrisoSys.Models;
using SorrisoSys.Repositories.Interfaces;

namespace SorrisoSys.Repositories
{
    public class ProcedimentoRepository : IProcedimentoRepository
    {
        private readonly SorrisoSysContext _context;

        public ProcedimentoRepository(SorrisoSysContext context)
        {
            _context = context;
        }

        public async Task<bool> ValidarProcedimentoExistenteAsync(Procedimento procedimento)
        {
            return await _context.Procedimentos.AnyAsync<Procedimento>(x => x.Nome.ToUpper().Equals(procedimento.Nome.ToUpper()));
        }
    }
}
