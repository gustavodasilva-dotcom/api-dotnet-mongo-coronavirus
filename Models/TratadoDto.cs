using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class TratadoDto
    {
        [Required]
        public DateTime DataDeNascimento { get; set; }
        
        [Required]
        public string Sexo { get; set; }

        [Required]
        public string Cpf { get; set; }

        [Required]
        public DateTime DataTesteNegativo { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }
    }
}