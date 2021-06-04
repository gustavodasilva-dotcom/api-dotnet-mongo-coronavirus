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
            bool existe = _infectadosCollection.Find(i => i.Cpf == dto.Cpf).Any();

            if (existe == true)
                return StatusCode(409, "Um infectado com esse CPF já existe!");

            var novoInfectado = new Infectado(dto.DataDeNascimento, dto.Sexo, dto.Cpf, dto.DataTestePositivo, dto.Latitude, dto.Longitude);

            _infectadosCollection.InsertOne(novoInfectado);

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

            bool existe = _infectadosCollection.Find(filtro).Any();

            if (existe == false)
                return StatusCode(404, "Não foi encontrado nenhum infectado com esse CPF!");

            var infectado = _infectadosCollection.Find(filtro).Limit(2).Single();

            return Ok(infectado);
        }

        [HttpPut("{cpfInfectado}")]
        public ActionResult AtualizarInfectado([FromBody] InfectadoDto dto, [FromRoute] string cpfInfectado)
        {   
            var filtro = Builders<Infectado>.Filter.Where(i => i.Cpf == cpfInfectado);

            bool existe = _infectadosCollection.Find(filtro).Any();

            if (existe == false)
                return StatusCode(404, "Não foi encontrado nenhum infectado com esse CPF!");

            var infectado = new Infectado(dto.DataDeNascimento, dto.Sexo, dto.Cpf, dto.DataTestePositivo, dto.Latitude, dto.Longitude);

            _infectadosCollection.ReplaceOne(filtro, infectado);

            return Ok("Infectado atualizado com sucesso!");
        }

        [HttpDelete("{cpfInfectado}")]
        public ActionResult DeletarInfectado([FromRoute] string cpfInfectado)
        {
            var filtro = Builders<Infectado>.Filter.Where(i => i.Cpf == cpfInfectado);

            bool existe = _infectadosCollection.Find(filtro).Any();

            if (existe == false)
                return StatusCode(404, "Não foi encontrado nenhum infectado com esse CPF!");

            _infectadosCollection.DeleteOne(filtro);

            return Ok("Infectado deletado com sucesso!");
        }
        
    }
    
}