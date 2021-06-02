using Api.Data.Collections;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

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

            var tratado = _tratadosCollection.Find(filtro).Limit(2).Single();

            return Ok(tratado);
        }

        [HttpPut("{cpfTratado}")]
        public ActionResult AtualizarTratado([FromBody] TratadoDto dto, [FromRoute] string cpfTratado)
        {
            var tratado = new Tratado(dto.DataDeNascimento, dto.Sexo, dto.Cpf, dto.DataTesteNegativo, dto.Latitude, dto.Longitude);

            var filtro = Builders<Tratado>.Filter.Where(t => t.Cpf == cpfTratado);

            _tratadosCollection.ReplaceOne(filtro, tratado);

            return Ok("Tratado atualizado com sucesso!");
        }

        [HttpDelete("{cpfTratado}")]
        public ActionResult DeletarTratado([FromRoute] string cpfTratado)
        {
            var filtro = Builders<Tratado>.Filter.Where(t => t.Cpf == cpfTratado);

            _tratadosCollection.DeleteOne(filtro);

            return Ok("Tratado deletado com sucesso!");
        }
    }
}