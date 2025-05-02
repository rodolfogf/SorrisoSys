using Moq;
using SorrisoSys.Models;
using SorrisoSys.Repositories.Interfaces;
using SorrisoSys.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace SorrisoSys.Tests.Moq
{
    public class PacienteMoqTests
    {
        private readonly Mock<IPacienteRepository> _pacienteRepositoryMock;
        private readonly PacienteController _pacienteController;
        private readonly PacienteGenerator _pacienteGenerator;

        public PacienteMoqTests()
        {
            _pacienteRepositoryMock = new Mock<IPacienteRepository>();
            _pacienteController = new PacienteController(null, _pacienteRepositoryMock.Object);
            _pacienteGenerator = new PacienteGenerator();
        }

        [Fact]
        public async Task DeveRetornarPacientePorId_DeveRetornarPaciente()
        {
            // Arrange
            var paciente = _pacienteGenerator.GerarPacienteFake();
            _pacienteRepositoryMock.Setup(x => x.GetPacienteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(paciente);

            // Act
            var result = await _pacienteController.RetornarPacientePorId(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(paciente, okResult.Value);

            // Verifica se o método do repositório foi chamado pelo menos uma vez
            _pacienteRepositoryMock.Verify(x => x.GetPacienteByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task RetornarPacientePorId_DeveChamarRepositorioPeloMenosUmaVez()
        {
            // Arrange
            var paciente = _pacienteGenerator.GerarPacienteFake();
            _pacienteRepositoryMock.Setup(x => x.GetPacienteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(paciente);

            // Act
            await _pacienteController.RetornarPacientePorId(1);

            // Assert
            _pacienteRepositoryMock.Verify(x => x.GetPacienteByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task CadastrarPaciente_DeveChamarValidarDadosPacientePeloMenosUmaVez()
        {
            // Arrange
            var paciente = _pacienteGenerator.GerarPacienteFake();
            _pacienteRepositoryMock.Setup(x => x.AddPacienteAsync(It.IsAny<Paciente>()))
                .ReturnsAsync(paciente);
            _pacienteRepositoryMock.Setup(x => x.ValidarDadosPaciente(It.IsAny<Paciente>()))
                .Returns(new ValidationResult());

            // Act
            await _pacienteController.CadastrarPaciente(paciente);

            // Assert
            _pacienteRepositoryMock.Verify(x => x.ValidarDadosPaciente(It.IsAny<Paciente>()), Times.Once);
        }

        [Fact]
        public async Task CadastrarPaciente_DeveChamarAddPacienteAsyncPeloMenosUmaVez()
        {
            // Arrange
            var paciente = _pacienteGenerator.GerarPacienteFake();
            _pacienteRepositoryMock.Setup(x => x.AddPacienteAsync(It.IsAny<Paciente>()))
                .ReturnsAsync(paciente);
            _pacienteRepositoryMock.Setup(x => x.ValidarDadosPaciente(It.IsAny<Paciente>()))
                .Returns(new ValidationResult());

            // Act
            await _pacienteController.CadastrarPaciente(paciente);

            // Assert
            _pacienteRepositoryMock.Verify(x => x.AddPacienteAsync(It.IsAny<Paciente>()), Times.Once);
        }
    }
}