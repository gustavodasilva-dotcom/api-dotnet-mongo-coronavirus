using Api.Data.Collections;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;
using System;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TratadoController : ControllerBase
    {
        Data.MongoDB _mongoDB;
        IMongoCollection<Tratado> _tratadosCollection;
        
        public TratadoController(Data.MongoDB mongoDB)
        {
            _mongoDB = mongoDB;
            _tratadosCollection = _mongoDB.DB.GetCollection<Tratado>(typeof(Tratado).Name.ToLower());
        }

        [HttpPost]
        public ActionResult SalvarTratado([FromBody] TratadoDto dto)
        {
            bool existe = _tratadosCollection.Find(t => t.Cpf == dto.Cpf).Any();

            if (existe == true)
                return StatusCode(409, "Um tratado com esse CPF já existe!");

            var tratado = new Tratado(dto.DataDeNascimento, dto.Sexo, dto.Cpf, dto.DataTesteNegativo, dto.Latitude, dto.Longitude);

            _tratadosCollection.InsertOne(tratado);

            return StatusCode(201, "Tratado adicionado com sucesso!");
        }

        [HttpGet]
        public ActionResult ObterTratados()
        {
            var tratados = _tratadosCollection.Find(Builders<Tratado>.Filter.Empty).ToList();

            return Ok(tratados);
        }

        [HttpGet("{cpfTratado}")]
        public ActionResult ObterTrtatados([FromRoute] string cpfTratado)
        {
            var filtro = Builders<Tratado>.Filter.Where(t => t.Cpf == cpfTratado);

            bool existe = _tratadosCollection.Find(filtro).Any();

            if (existe == false)
                return StatusCode(404, "Não foi encontrado nenhum tratado com esse CPF!");

            var tratado = _tratadosCollection.Find(filtro).Limit(2).Single();

            return Ok(tratado);
        }

        [HttpPut("{cpfTratado}")]
        public ActionResult AtualizarTratado([FromBody] TratadoDto dto, [FromRoute] string cpfTratado)
        {
            var filtro = Builders<Tratado>.Filter.Where(t => t.Cpf == cpfTratado);

            bool existe = _tratadosCollection.Find(filtro).Any();

            if (existe == false)
                return StatusCode(404, "Não foi encontrado nenhum tratado com esse CPF!");

            var tratado = new Tratado(dto.DataDeNascimento, dto.Sexo, dto.Cpf, dto.DataTesteNegativo, dto.Latitude, dto.Longitude);

            _tratadosCollection.ReplaceOne(filtro, tratado);

            return Ok("Tratado atualizado com sucesso!");
        }

        [HttpPatch("{cpfTratado}/data-de-nascimento/{dataDeNascimento}")]
        public ActionResult AtualizarTratado([FromRoute] string cpfTratado, [FromRoute] DateTime dataDeNascimento)
        {
            var filtro = Builders<Tratado>.Filter.Where(t => t.Cpf == cpfTratado);

            bool existe = _tratadosCollection.Find(filtro).Any();

            if (existe == false)
                return StatusCode(404, "Não foi encontrado nenhum tratado com esse CPF!");
            
            var atualizar = Builders<Tratado>.Update.Set(t => t.DataDeNascimento, dataDeNascimento);

            _tratadosCollection.UpdateOne(filtro, atualizar);

            return Ok("Data de nascimento atualizada com sucesso!");
        }

        [HttpPatch("{cpfTratado}/sexo/{sexo}")]
        public ActionResult AtualizarTratado([FromRoute] string cpfTratado, string sexo)
        {
            var filtro = Builders<Tratado>.Filter.Where(t => t.Cpf == cpfTratado);

            bool existe = _tratadosCollection.Find(filtro).Any();

            if (existe == false)
                return StatusCode(404, "Não foi encontrado nenhum tratado com esse CPF!");

            var atualizar = Builders<Tratado>.Update.Set(t => t.Sexo, sexo);

            _tratadosCollection.UpdateOne(filtro, atualizar);

            return Ok("Sexo atualizado com sucesso!");
        }

        [HttpPatch("{cpfTratado}/cpf/{cpf}")]
        public ActionResult AutalizarTratadoCpf([FromRoute] string cpfTratado, string cpf)
        {
            var filtro = Builders<Tratado>.Filter.Where(t => t.Cpf == cpfTratado);

            bool existe = _tratadosCollection.Find(filtro).Any();

            if (existe == false)
                return StatusCode(404, "Não foi encontrado nenhum tratado com esse CPF!");

            var atualizar = Builders<Tratado>.Update.Set(t => t.Cpf, cpf);

            _tratadosCollection.UpdateOne(filtro, atualizar);

            return Ok("CPF atualizado com sucesso!");
        }

        [HttpPatch("{cpfTratado}/data-teste-negativo/{dataTesteNegativo}")]
        public ActionResult AtualizarTratadoDataTesteNegativo([FromRoute] string cpfTratado, [FromRoute] DateTime dataTesteNegativo)
        {
            var filtro = Builders<Tratado>.Filter.Where(t => t.Cpf == cpfTratado);

            bool existe = _tratadosCollection.Find(filtro).Any();

            if (existe == false)
                return StatusCode(404, "Não foi encontrado nenhum tratado com esse CPF!");
            
            var atualizar = Builders<Tratado>.Update.Set(t => t.DataTesteNegativo, dataTesteNegativo);

            _tratadosCollection.UpdateOne(filtro, atualizar);

            return Ok("Data de teste negativo atualizado com sucesso!");
        }

        [HttpPatch("{cpfTratado}/localizacao/{latitude:double}/{longitude:double}")]
        public ActionResult AtualizarTratado([FromRoute] string cpfTratado, [FromRoute] double longitude, [FromRoute] double latitude)
        {
            var filtro = Builders<Tratado>.Filter.Where(t => t.Cpf == cpfTratado);

            bool existe = _tratadosCollection.Find(filtro).Any();

            if (existe == false)
                return StatusCode(404, "Não foi encontrado nenhum tratado com esse CPF!");
            
            var localizacao = new GeoJson2DGeographicCoordinates(longitude, latitude);

            var atualizar = Builders<Tratado>.Update.Set(t => t.Localizacao, localizacao);

            _tratadosCollection.UpdateOne(filtro, atualizar);

            return Ok("Localização atualizada com sucesso!");
        }

        [HttpDelete("{cpfTratado}")]
        public ActionResult DeletarTratado([FromRoute] string cpfTratado)
        {
            var filtro = Builders<Tratado>.Filter.Where(t => t.Cpf == cpfTratado);

            bool existe = _tratadosCollection.Find(filtro).Any();

            if (existe == false)
                return StatusCode(404, "Não foi encontrado nenhum tratado com esse CPF!");

            _tratadosCollection.DeleteOne(filtro);

            return Ok("Tratado deletado com sucesso!");
        }
    }
}