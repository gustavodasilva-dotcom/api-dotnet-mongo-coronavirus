using System;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Api.Data.Collections
{
    public class Tratado
    {
        public DateTime DataDeNascimento { get; set; }
        public string Sexo { get; set; }
        public string Cpf { get; set; }
        public DateTime DataTesteNegativo { get; set; }
        public GeoJson2DGeographicCoordinates Localizacao { get; set; }

        public Tratado(DateTime dataDeNascimento, string sexo, string cpf, DateTime dataTesteNegativo, double latitude, double longitude)
        {
            DataDeNascimento = dataDeNascimento;
            Sexo = sexo;
            Cpf = cpf;
            DataTesteNegativo = dataTesteNegativo;
            Localizacao = new GeoJson2DGeographicCoordinates(longitude, latitude);
        }
    }
}