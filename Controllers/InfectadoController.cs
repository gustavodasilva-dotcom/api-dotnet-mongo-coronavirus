using Api.Data.Collections;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InfectadoController : ControllerBase
    {
        Data.MongoDB _mongoDB;
        IMongoCollection<Infectado> _infectadosCollection;

        public InfectadoController(Data.MongoDB mongoDB)
        {
            _mongoDB = mongoDB;
            _infectadosCollection = _mongoDB.DB.GetCollection<Infectado>(typeof(Infectado).Name.ToLower());
        }

        [HttpPost]
        public ActionResult SalvarInfectado([FromBody] InfectadoDto dto)
        {
            var infectado = new Infectado(dto.DataDeNascimento, dto.Sexo, dto.Cpf, dto.DataTestePositivo, dto.Latitude, dto.Longitude);

            _infectadosCollection.InsertOne(infectado);

            return StatusCode(201, "Infectado adicionado com sucesso!");
        }

        [HttpGet]
        public ActionResult ObterInfectados()
        {
            var filtro = Builders<Infectado>.Filter.Empty;

            var infectados = _infectadosCollection.Find(filtro).ToList();

            return Ok(infectados);
        }

        [HttpGet("{cpfInfectado}")]
        public ActionResult ObterInfectados(string cpfInfectado)
        {
            var filtro = Builders<Infectado>.Filter.Where(i => i.Cpf == cpfInfectado);

            var infectado = _infectadosCollection.Find(filtro).Limit(2).Single();

            return Ok(infectado);
        }

        [HttpPut("{cpfInfectado}")]
        public ActionResult AtualizarInfectado([FromBody] InfectadoDto dto, [FromRoute] string cpfInfectado)
        {
            var infectado = new Infectado(dto.DataDeNascimento, dto.Sexo, dto.Cpf, dto.DataTestePositivo, dto.Latitude, dto.Longitude);
            
            var filtro = Builders<Infectado>.Filter.Where(i => i.Cpf == cpfInfectado);

            _infectadosCollection.ReplaceOne(filtro, infectado);

            return Ok("Infectado atualizado com sucesso!");
        }

        [HttpDelete("{cpfInfectado}")]
        public ActionResult DeletarInfectado([FromRoute] string cpfInfectado)
        {
            var filtro = Builders<Infectado>.Filter.Where(i => i.Cpf == cpfInfectado);

            _infectadosCollection.DeleteOne(filtro);

            return Ok("Infectado deletado com sucesso!");
        }
        
    }
    
}