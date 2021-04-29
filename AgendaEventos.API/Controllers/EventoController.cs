using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AgendaEventos.API.Dtos;
using AgendaEventos.Dominio;
using AgendaEventos.Repositorio;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProAgil.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class EventoController : ControllerBase
    {
        private readonly IAgendaEventosRepositorio _repo;
        private readonly IMapper _mapper;

        public EventoController(IAgendaEventosRepositorio repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
                var eventos = _repo.GetAllEventosAsync();
                var results = _mapper.Map<IEnumerable<EventoDto>>(eventos);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro no Banco de Dados {ex.Message}");
            }
        }

        [HttpGet("{eventoId}")]
        public async Task<ActionResult> Get(int eventoId)
        {
            try
           {
                var evento = await _repo.GetEventoByIdAsync(eventoId);
                var results = _mapper.Map<EventoDto>(evento);
                return Ok(results);
            }
            catch (Exception ex)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro no Banco de Dados {ex.Message}");
            }
        }

        [HttpGet("getByTema/{tema}")]
        public async Task<ActionResult> Get(string tema)
        {
            try
            {
                var eventos = _repo.GetAllEventosByTemaAsync(tema);
                return Ok(eventos);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao filtrar os temas!");
            }
        }

        [HttpGet("getByData/{dataInicio}/{dataFim}")]
        public async Task<ActionResult> GetData(string dataInicio, string dataFim)
        {
            try
            {
                var eventos = _repo.GetAllEventosByDataAsync(dataInicio, dataFim);
                return Ok(eventos);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao filtrar os Eventos por Data!");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post(EventoDto model)
        {
            try
            {
                var evento = _mapper.Map<Evento>(model);
                var QtdtipoExclusivo =  _repo.GetQtdEventoTipoExclusivo();
                var tipo = evento.Tipo.Equals("E");
                    
                
                    _repo.Add(evento);

                    if (await _repo.SaveChangesAsync())
                    {
                        var result = Created($"/api/evento/{model.Id}", _mapper.Map<EventoDto>(evento));

                        var dados = new UsuarioEventoDto {UsuarioId = model.UsuarioId, EventoId = evento.Id};
                        await PostEventoParticipar(dados);

                        return result;
                    }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao inserir informações no Banco de Dados\n{ex.Message}");
            }

            return BadRequest();
        }

        [HttpPost("postByEventoParticipar")]
        public async Task<ActionResult> PostEventoParticipar(UsuarioEventoDto model)
        {
            try
            {
                
                var usuarioEvento = _mapper.Map<UsuarioEvento>(model);
                _repo.Add(usuarioEvento);

                if (await _repo.SaveChangesAsync())
                {
                    return Created($"/api/evento/{model.EventoId}", _mapper.Map<UsuarioEventoDto>(usuarioEvento));
                }

            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao inserir informações no Banco de Dados\n{ex.Message}");
            }

            return BadRequest();
        }

         [HttpPut("{EventoId}")]
        public async Task<IActionResult> Put(int EventoId, EventoDto model)
        {
            try
            {
                var evento = await _repo.GetEventoByIdAsync(EventoId);
                if (evento == null) return NotFound();

                _mapper.Map(model, evento);

                _repo.Update(evento);

                if (await _repo.SaveChangesAsync())
                {
                    return Created($"/api/evento/{model.Id}", _mapper.Map<EventoDto>(evento));

                }
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao atualizar o evento: " + ex.Message);
            }

            return BadRequest();
        }

        [HttpDelete("{eventoId}")]
        public async Task<ActionResult> Delete(int eventoId)
        {
            try
            {
                var evento = await _repo.GetEventoByIdAsync(eventoId);
                if (evento == null) return NotFound();
                _repo.Delete(evento);

                if (await _repo.SaveChangesAsync())
                {
                    return Ok(new { message = "Deletado" });
                }else{
                    throw new Exception("Ocorreu um problem não específico ao tentar deletar Evento.");
                }

            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar deletar o evento \n{ex.Message}");
            }

            return BadRequest();
        }
    }
}