using MongoDB.Driver.GeoJsonObjectModel;
using System;

namespace ApiMongo.Data.Collections
{
    public class Infectado
    {
        public Guid Id { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Sexo { get; set; }
        public GeoJson2DGeographicCoordinates Localizacao { get; set; }

        public Infectado(DateTime dataNascimento, string sexo, double latitude, double longitude)
        {
            this.Id = Guid.NewGuid();
            this.DataNascimento = dataNascimento;
            this.Sexo = sexo;
            this.Localizacao = new GeoJson2DGeographicCoordinates(longitude, latitude);
        }

        public Infectado(Guid id, DateTime dataNascimento, string sexo, double latitude, double longitude)
        {
            this.Id = id;
            this.DataNascimento = dataNascimento;
            this.Sexo = sexo;
            this.Localizacao = new GeoJson2DGeographicCoordinates(longitude, latitude);
        }
    }
}
