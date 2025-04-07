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
            if(cpf == null || string.IsNullOrWhiteSpace(cpf) || cpf.Length != 11) 
                return false;

            string numeroBase = cpf.Substring(0, 9);
            string dv = cpf.Substring(9, 2);
            int somaNumeroBase = 0;

            for (int i = 0; i<numeroBase.Length; i++)
            {
                int valorDigito = (int)char.GetNumericValue(numeroBase[i]);
                somaNumeroBase += valorDigito * (10-i);
            }

            int restoDivisao = somaNumeroBase % 11;
            string dvCalculado = string.Empty;

            if (restoDivisao == 0 || restoDivisao == 1)
                dvCalculado = "0";
            else
                dvCalculado = (11 - restoDivisao).ToString();

            somaNumeroBase = 0;
            numeroBase = $"{numeroBase}{dvCalculado}";

            for (int i = 0; i < numeroBase.Length; i++)
            {
                int valorDigito = (int)char.GetNumericValue(numeroBase[i]);
                somaNumeroBase += valorDigito * (11 - i);
            }

            restoDivisao = somaNumeroBase % 11;
            if (restoDivisao == 0 || restoDivisao == 1)
                dvCalculado = $"{dvCalculado}0";
            else
                dvCalculado = $"{dvCalculado}{(11 - restoDivisao)}";

            return dv[0] == dvCalculado[0] && dv[1] == dvCalculado[1];
        }
    }
}
