using Microsoft.EntityFrameworkCore;
using SorrisoSys.Data;
using SorrisoSys.Repositories.Interfaces;

namespace SorrisoSys.Repositories
{
    public class PacienteRepository : IPacienteRepository
    {
        private readonly SorrisoSysContext _context;

        public PacienteRepository(SorrisoSysContext context)
        {
            _context = context;
        }

        public async Task<bool> ValidarPacienteExistenteAsync(int id)
        {
            return await _context.Pacientes.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> ValidarCpfExistenteAsync(string? cpf)
        {
            if (string.IsNullOrEmpty(cpf)) return false;

            var paciente = await _context.Pacientes.FirstOrDefaultAsync(x => x.Cpf == cpf);
            return paciente != null;
        }
    }
}
