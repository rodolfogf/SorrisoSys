using SorrisoSys.Models;
using Xunit;
using FluentValidation.TestHelper;

namespace SorrisoSys.Tests
{
    public class PacienteTests
    {
        private readonly PacienteValidator _validator;

        public PacienteTests()
        {
            _validator = new PacienteValidator();
        }

        [Theory]
        [InlineData("12345678901")] // CPF com dígitos iguais (inválido)
        [InlineData("11111111111")] // CPF inválido
        [InlineData("123")] // CPF muito curto
        [InlineData("")] // CPF vazio
        [InlineData(null)] // CPF nulo
        public void DeveRetornarErro_QuandoCPFInvalido(string cpfInvalido)
        {
            // Arrange
            var paciente = new Paciente { Cpf = cpfInvalido };

            // Act
            var result = _validator.TestValidate(paciente);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Cpf)
                  .WithErrorMessage("CPF inválido");
        }

        [Theory]
        [InlineData("1234567")] // CEP muito curto
        [InlineData("123456789")] // CEP muito longo
        [InlineData("ABCDEFGH")] // CEP com letras
        [InlineData("")] // CEP vazio
        [InlineData(null)] // CEP nulo
        public void DeveRetornarErro_QuandoCEPInvalido(string cepInvalido)
        {
            // Arrange
            var paciente = new Paciente { Cep = cepInvalido };

            // Act
            var result = _validator.TestValidate(paciente);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Cep)
                  .WithErrorMessage("Formato de CEP inválido. Digite apenas números");
        }

        [Theory]
        [InlineData("")] // Nome vazio
        [InlineData(null)] // Nome nulo
        [InlineData("   ")] // Nome com espaços em branco
        public void NaoDeveRetornarErro_QuandoNomeVazio(string nomeVazio)
        {
            // Arrange
            var paciente = new Paciente { Nome = nomeVazio };

            // Act
            var result = _validator.TestValidate(paciente);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Nome);
        }

        [Fact]
        public void DevePassar_QuandoCPFValido()
        {
            // Arrange
            var paciente = new Paciente { Cpf = "52998224725" }; // CPF válido

            // Act
            var result = _validator.TestValidate(paciente);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Cpf);
        }

        [Fact]
        public void DevePassarQuando_CEPValido()
        {
            // Arrange
            var paciente = new Paciente { Cep = "01001000" }; // CEP válido

            // Act
            var result = _validator.TestValidate(paciente);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Cep);
        }
    }
}