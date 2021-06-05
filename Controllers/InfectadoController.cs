using System;
using Api.Data.Collections;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;

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

        [HttpPatch("{cpfInfectado}/data-de-nascimento/{dataDeNascimento}")]
        public ActionResult AtualizarInfectado([FromRoute] string cpfInfectado, DateTime dataDeNascimento)
        {
            var filtro = Builders<Infectado>.Filter.Where(i => i.Cpf == cpfInfectado);

            bool existe = _infectadosCollection.Find(filtro).Any();

            if (existe == false)
                return StatusCode(404, "Não foi encontrado nenhum infectado com esse CPF!");

            var atualizar = Builders<Infectado>.Update.Set(i => i.DataDeNascimento, dataDeNascimento);

            _infectadosCollection.UpdateOne(filtro, atualizar);
            
            return Ok("Data de nascimento atualizada com sucesso!");
        }

        [HttpPatch("{cpfInfectado}/sexo/{sexo}")]
        public ActionResult AtualizarInfectado([FromRoute] string cpfInfectado, string sexo)
        {
            var filtro = Builders<Infectado>.Filter.Where(i => i.Cpf == cpfInfectado);

            bool existe = _infectadosCollection.Find(filtro).Any();

            if (existe == false)
                return StatusCode(404, "Não foi encontrado nenhum infectado com esse CPF!");

            var atualizar = Builders<Infectado>.Update.Set(i => i.Sexo, sexo);

            _infectadosCollection.UpdateOne(filtro, atualizar);

            return Ok("Sexo atualizado com sucesso!");
        }

        [HttpPatch("{cpfInfectado}/cpf/{cpf}")]
        public ActionResult AtualizarInfectadoCpf([FromRoute] string cpfInfectado, string cpf)
        {
            var filtro = Builders<Infectado>.Filter.Where(i => i.Cpf == cpfInfectado);

            bool existe = _infectadosCollection.Find(filtro).Any();

            if (existe == false)
                return StatusCode(404, "Não foi encontrado nenhum infectado com esse CPF!");

            var atualizar = Builders<Infectado>.Update.Set(i => i.Cpf, cpf);

            _infectadosCollection.UpdateOne(filtro, atualizar);

            return Ok("CPF atualizado com sucesso!");
        }

        [HttpPatch("{cpfInfectado}/data-teste-positivo/{dataTestePositivo}")]
        public ActionResult AtualizarInfectadoDataTestePositivo([FromRoute] string cpfInfectado, [FromRoute] DateTime dataTestePositivo)
        {
            var filtro = Builders<Infectado>.Filter.Where(i => i.Cpf == cpfInfectado);

            bool existe = _infectadosCollection.Find(filtro).Any();

            if (existe == false)
                return StatusCode(404, "Não foi encontrado nenhum infectado com esse CPF!");

            var atualizar = Builders<Infectado>.Update.Set(i => i.DataTestePositivo, dataTestePositivo);

            _infectadosCollection.UpdateOne(filtro, atualizar);

            return Ok("Data de teste positivo atualizada com sucesso!");
        }

        [HttpPatch("{cpfInfectado}/localizacao/{latitude:double}/{longitude:double}")]
        public ActionResult AtualizarInfectado([FromRoute] string cpfInfectado, [FromRoute] double longitude, [FromRoute] double latitude)
        {
            var filtro = Builders<Infectado>.Filter.Where(i => i.Cpf == cpfInfectado);

            bool existe = _infectadosCollection.Find(filtro).Any();

            if (existe == false)
                return StatusCode(404, "Não foi encontrado nenhum infectado com esse CPF!");

            var localizacao = new GeoJson2DGeographicCoordinates(longitude, latitude);

            var atualizar = Builders<Infectado>.Update.Set(i => i.Localizacao, localizacao);

            _infectadosCollection.UpdateOne(filtro, atualizar);

            return Ok("Localização atualizada com sucesso!");
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