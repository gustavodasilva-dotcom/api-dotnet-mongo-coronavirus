using System;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Api.Data.Collections
{
    public class Infectado
    {
        public DateTime DataDeNascimento { get; set; }
        public string Sexo { get; set; }
        public string Cpf { get; set; }
        public DateTime DataTestePositivo { get; set; }
        public GeoJson2DGeographicCoordinates Localizacao { get; set; }

        public Infectado(DateTime dataDeNascimento, string sexo, string cpf, DateTime dataTestePositivo, double latitude, double longitude)
        {
            DataDeNascimento = dataDeNascimento;
            Sexo = sexo;
            Cpf = cpf;
            DataTestePositivo = dataTestePositivo;
            Localizacao = new GeoJson2DGeographicCoordinates(longitude, latitude);
        }
    }
}