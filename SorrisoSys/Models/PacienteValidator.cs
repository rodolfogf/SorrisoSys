using FluentValidation;

namespace SorrisoSys.Models
{
    public class PacienteValidator : AbstractValidator<Paciente>
    {
        public PacienteValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress();
            RuleFor(x => x.Cep)
                .Matches(@"^\d{8}$")
                .WithMessage("Formato de CEP inválido. Digite apenas números");
            RuleFor(x => x.Telefone)
                .Matches(@"^\d{10}$|^\d{11}$")
                .WithMessage("Formato número de telefone inválido. Digite apenas números");
            RuleFor(x => x.Cpf)
                .Matches(@"^\d{11}").WithMessage("Formato de CPF inválido. Digite apenas números")
                .Must(ValidaCpf).WithMessage("CPF inválido");
        }

        private bool ValidaCpf(string cpf)
        {
            return true;
        }
    }
}
