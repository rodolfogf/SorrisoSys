﻿using System.ComponentModel.DataAnnotations;

namespace SorrisoSys.Models
{
    public class Paciente
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public string? Cpf { get; set; }
        public string? Cep { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Logradouro { get; set; }
        public string? Estado { get; set; }
        public string? Cidade { get; set; }        
    }
}
