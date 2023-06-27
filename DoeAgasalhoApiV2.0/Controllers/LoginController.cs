using DoeAgasalhoApiV2._0.Exceptions;
using DoeAgasalhoApiV2._0.Models.CustomModels;
using DoeAgasalhoApiV2._0.Repositories.Interface;
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
        private readonly IPontoColetaService _pontoColetaService;

        public LoginController(IUsuarioRepository usuarioRepository, ILoginService loginService, IUsuarioService usuarioService, ITokenService tokenService, IPontoColetaService pontoColetaService)
        {
            _usuarioRepository = usuarioRepository;
            _loginService = loginService;
            _usuarioService = usuarioService;
            _tokenService = tokenService;
            _pontoColetaService = pontoColetaService;   
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

            try
            {
                //Verifica se o ponto de coleta do usuario esta ativo, se estiver inativo não conseguirá fazer login
                if (!_pontoColetaService.IsActiveCollectPoint(usuario) && usuario.Tipo != "admin")
                {
                    return NotFound(new { Message = "Você não está associado a nenhum ponto de coleta, contate o administrador." });
                }
            }
            catch (BusinessOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
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
