using DoeAgasalhoApiV2._0.Entities;
using DoeAgasalhoApiV2._0.Models;
using DoeAgasalhoApiV2._0.Repository.Interface;
using DoeAgasalhoApiV2._0.Services;
using DoeAgasalhoApiV2._0.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoeAgasalhoApiV2._0.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILoginService _loginService;
        private readonly IUsuarioService _usuarioService;
        private readonly ITokenService _tokenService;

        public LoginController(IUsuarioRepository usuarioRepository, ILoginService loginService, IUsuarioService usuarioService, ITokenService tokenService)
        {
            _usuarioRepository = usuarioRepository;
            _loginService = loginService;
            _usuarioService = usuarioService;
            _tokenService = tokenService;

        }

        [HttpPost]
        [Route("signin")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody] LoginModel model)
        {
            var usuario = _usuarioRepository.GetByEmailAndPassword(model.Email, model.Senha);

            //Verifica se usuario esta ativo
            

            if (usuario == null || !_loginService.VerifyPassword(model.Senha, usuario.Senha))
            {
                return NotFound(new { Message = "Usuário ou senha inválidos!" });
            }

            if (!_usuarioService.IsActiveUser(usuario))
            {
                return Unauthorized(new { Message = "Usuário inativo. Entre em contato com o administrador." });
            }

            //Gera o token e esconde a senha
            var token = _tokenService.GenerateToken(usuario);
            usuario.Senha = "";
            return new
            {
                usuario = usuario,
                token = token,
                idUsuario = usuario.Id
            };
        }

    }
}
