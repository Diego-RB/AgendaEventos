using System;
using System.Threading.Tasks;
using AgendaEventos.API.Dtos;
using AgendaEventos.Repositorio;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgendaEventos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IAgendaEventosRepositorio _repo;
        private readonly IMapper _mapper;

        public UsuarioController(IAgendaEventosRepositorio repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet("{usuarioId}")]
        public async Task<ActionResult> Get(int usuarioId)
        {
            try
           {
                var usuario = await _repo.GetUsuarioByIdAsync(usuarioId);
                var results = _mapper.Map<UsuarioDto>(usuario);
                return Ok(results);
            }
            catch (Exception ex)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro no Banco de Dados {ex.Message}");
            }
        }

        [HttpGet("getByNome/{nomeUsuario}")]
        public async Task<ActionResult> Get(string nomeUsuario)
        {
            try
           {
                var usuario = await _repo.GetUsuarioByNomeAsync(nomeUsuario);
                var results = _mapper.Map<UsuarioDto>(usuario);
                return Ok(results);
            }
            catch (Exception ex)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro no Banco de Dados {ex.Message}");
            }
        }
    }
}