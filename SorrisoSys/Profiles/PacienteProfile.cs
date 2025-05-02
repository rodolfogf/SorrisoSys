using AutoMapper;
using SorrisoSys.Data.DTOs;
using SorrisoSys.Models;

namespace SorrisoSys.Profiles
{
    public class PacienteProfile : Profile
    {
        public PacienteProfile()
        {
            CreateMap<CreatePacienteDTO, Paciente>();
            CreateMap<UpdatePacienteDTO, Paciente>();
            CreateMap<Paciente, UpdatePacienteDTO>();
            CreateMap<Paciente, ReadPacienteDTO>();
        }
    }
}