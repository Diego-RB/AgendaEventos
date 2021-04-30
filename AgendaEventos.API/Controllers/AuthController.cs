using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using AgendaEventos.Dominio.Identity;
using AgendaEventos.Repositorio;
using AgendaEventos.API.Dtos;

namespace AgendaEventos.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  [AllowAnonymous]
  public class AuthController : ControllerBase
  {
    private readonly IConfiguration _config;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IAgendaEventosRepositorio _repo;
    private readonly IMapper _mapper;
    




    public AuthController(IConfiguration config,
                          UserManager<User> userManager,
                          SignInManager<User> signInManager,
                          IAgendaEventosRepositorio repo,
                          IMapper mapper)
    {
        _signInManager = signInManager;
        _repo = repo;
        _mapper = mapper;
        _config = config;
        _userManager = userManager;
      
    }

    [HttpGet("GetUser")]
    public async Task<IActionResult> GetUser()
    {
        return Ok(new UserDto());
    }


    [HttpGet("{userId1}")]
    public async Task<IActionResult> Get(float userId1)
    {
        try
        {
            var userId = (int)userId1;
            var user = await _repo.GetUsuarioByIdAsync(userId);
            var results = _mapper.Map<UserDto>(user);
            return Ok(results);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao carregar Usuario {ex.Message}");
        }
    }

    [HttpPut("{userId}")]
        public async Task<ActionResult> Put(int userId, UserDto model)
        {
            try
            {
                var user = await _repo.GetUsuarioByIdAsync(userId);
                if (user == null) return NotFound();

                _mapper.Map(model, user);
                _repo.Update(user);

                if (await _repo.SaveChangesAsync())
                {
                    //rota URL Link Created
                    // Pesquisar Unit of work com transAction
                    return Created($"/api/evento/{model.Id}", _mapper.Map<UserDto>(user));
                    //return Created(Url.Link("GetEventoId", new { id = evento.Id }), evento.Id);
                    //return Created($"/api/evento/{evento.Id}", evento.Id);
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao inserir informações no Banco de Dados\n{ex.Message}");
            }

            return BadRequest();
        }

    [HttpPost("Register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(UserDto userDto)
    {
        try
        {
            var user = _mapper.Map<User>(userDto);

            var result = await _userManager.CreateAsync(user ,userDto.Password);

            var userToReturn = _mapper.Map<UserDto>(user);

            if (result.Succeeded) 
            {
                return Created("GetUser", userToReturn);
            }
            
            return BadRequest(result.Errors);
        }
        catch (System.Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco Dados Falhou {ex.Message}");
        }
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(UserLoginDto userLogin)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(userLogin.UserName);

            var result = await _signInManager.CheckPasswordSignInAsync(user, userLogin.Password, false);

            if (result.Succeeded)
            {
                var appUser = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.NormalizedUserName == userLogin.UserName.ToUpper());

                var userToReturn = _mapper.Map<UserLoginDto>(appUser);

                return Ok(new {
                    token = GenerateJWToken(appUser).Result,
                    user = userToReturn
                });
            }

            return Unauthorized();
        }
        catch (System.Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao efetuar login: {ex.Message}");
        }
    }

    private async Task<string> GenerateJWToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName)
            //new Claim(ClaimTypes.Email, user.Email),
            //new Claim(ClaimTypes.MobilePhone, user.PhoneNumber)
        };

        var roles = await _userManager.GetRolesAsync(user);

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.ASCII
            .GetBytes(_config.GetSection("AppSettings:Token").Value));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
  }
}