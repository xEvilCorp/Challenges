using MongoDB.Driver.GeoJsonObjectModel;
using System;

namespace ApiMongo.Data.Collections
{
    public class InfectadoDTO
    {
        public Guid Id { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Sexo { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
