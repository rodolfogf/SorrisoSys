using Bogus;
using SorrisoSys.Models;

namespace SorrisoSys.Tests
{
    internal class PacienteGenerator
    {
        private const string _idioma = "pt_BR";

        public Paciente GerarPacienteFake()
        {
            var faker = new Faker<Paciente>(_idioma)
                .RuleFor(p => p.Nome, f => f.Name.FullName())
                .RuleFor(p => p.Cpf, f => f.Internet.Email())
                .RuleFor(p => p.DataNascimento, f => f.Date.Between(new DateTime(1950, 1, 1), new DateTime(2023, 12, 12)))
                .RuleFor(p => p.Telefone, f => f.Phone.PhoneNumber("(##) 9####-####"));

            return faker.Generate();
        }
    }
}