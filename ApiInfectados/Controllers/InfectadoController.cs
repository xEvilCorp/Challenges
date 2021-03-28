using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.GeoJsonObjectModel;
using System;
using ApiMongo.Data.Collections;
using MongoDB.Driver;

namespace ApiMongo.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class InfectadoController : ControllerBase
    {
        DBContext mongoDB;
        IMongoCollection<Infectado> infectadosCollection;

        public InfectadoController(DBContext db)
        {
            mongoDB = db;
            infectadosCollection = mongoDB.DB.GetCollection<Infectado>(typeof(Infectado).Name.ToLower());
        }

        [HttpPost]
        public ActionResult SalvarInfectado([FromBody] InfectadoDTO dto)
        {
            var infectado = new Infectado(dto.DataNascimento, dto.Sexo, dto.Latitude, dto.Longitude);

            infectadosCollection.InsertOne(infectado);

            return StatusCode(201, "Infectado adicionado com sucesso");
        }


        [HttpGet]
        public ActionResult ObterInfectados()
        {
            var infectados = infectadosCollection.Find(Builders<Infectado>.Filter.Empty).ToList();

            return Ok(infectados);
        }

        [HttpDelete]
        public ActionResult Delete([FromBody] InfectadoDTO dto)
        {
            infectadosCollection.DeleteOne(Builders<Infectado>.Filter.Where(x => x.Id == dto.Id));
            return StatusCode(201, "Infectado apagado com sucesso");
        }

        [HttpPut]
        public ActionResult UpdateInfectado([FromBody] InfectadoDTO dto)
        {
            var infectado = new Infectado(dto.Id, dto.DataNascimento, dto.Sexo, dto.Latitude, dto.Longitude);

            infectadosCollection.ReplaceOne(Builders<Infectado>.Filter.Where(x => x.Id == dto.Id), infectado);

            return StatusCode(201, "Infectado atualizado com sucesso");
        }
    }
}
