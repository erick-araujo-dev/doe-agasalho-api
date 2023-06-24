﻿using Microsoft.AspNetCore.Mvc;
using DoeAgasalhoApiV2._0.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using DoeAgasalhoApiV2._0.Exceptions;
using DoeAgasalhoApiV2._0.Models.CustomModels;
using DoeAgasalhoApiV2._0.Models.Entities;

namespace DoeAgasalhoApiV2._0.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;


        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }
        [HttpGet("all")]
        [Authorize(Roles = "admin")]
        public IActionResult GetAllUsers()
        {
            try
            {
                var users = _usuarioService.GetAllUsers();
                return Ok(users);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao obter todos os usuários.");
            }
        }

        [HttpGet("active")]
        [Authorize(Roles = "admin")]
        public IActionResult GetActiveUsers()
        {
            try
            {
                var users = _usuarioService.GetActiveUsers();
                return Ok(users);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao obter os usuários ativos.");
            }
        }

        [HttpGet("inactive")]
        [Authorize(Roles = "admin")]
        public IActionResult GetInactiveUsers()
        {
            try
            {
                var users = _usuarioService.GetInactiveUsers();
                return Ok(users);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao obter os usuários inativos.");
            }
        }

        [HttpPost("create")]
        [Authorize(Roles = "admin")]
        public ActionResult<Usuario> CreateNewUser(UsuarioCreateModel usuario)
        {
            try
            {
                var newUser = _usuarioService.CreateUser(usuario);
                return Ok(newUser);
            }
            catch (BusinessOperationException)
            {
                return StatusCode(409, "O email fornecido já está sendo utilizado por outro usuário. Por favor, utilize outro email.");
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, $"Tipo de dado inválido: {ex.Message}");
            }
            catch (NotFoundException)
            {
                return StatusCode(404, "Ponto de coleta não encontrado");
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao adicionar o usuário.");
            }
        }

        [HttpPut("{id}/updateusername")]
        public ActionResult<Usuario> UpdateUsername(int id, UpdateUsernameModel usuario)
        {
            try
            {
                var requestingUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                int requestingUserId = int.Parse(requestingUserIdClaim.Value);
                var updatedUser = _usuarioService.UpdateUsername(id, requestingUserId, usuario);
                return Ok(updatedUser);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, $"Erro de autorização: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, $"Tipo de dado inválido: {ex.Message}");
            }
            catch (NotFoundException)
            {
                return StatusCode(404, "Usuario não encontrado");
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao adicionar o usuário.");
            }
        }

        [HttpPut("{id}/changecollectpoint")]
        [Authorize(Roles = "admin")]
        public ActionResult<Usuario> Change(int id, ChangeCollectPointModel usuario)
        {
            try
            {
                var updatedUser = _usuarioService.ChangeCollectPoint(id, usuario);
                return Ok(updatedUser);
            }
            catch (NotFoundException)
            {
                return StatusCode(404, "Ponto de coleta não encontrado");
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, $"Tipo de dado inválido: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao adicionar o usuário.");
            }

        }

        [HttpPost("{id}/changepassword")]
        public IActionResult ChangePassword(int id, [FromBody] ChangePasswordModel user)
        {
            try
            {
                var requestingUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                int requestingUserId = int.Parse(requestingUserIdClaim.Value);

                var updatedUser = _usuarioService.ChangePassword(id, user, requestingUserId);
                return Ok(updatedUser);
            }
            catch (NotFoundException)
            {
                return StatusCode(404, "Usuario não encontrado");
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, $"Erro de validacao: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, $"Tipo de dado inválido: {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, $"Erro de autorização: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("{id}/activate")]
        [Authorize(Roles = "admin")]
        public IActionResult ActivateUser(int id)
        {
            try
            {
                _usuarioService.ActivateUser(id);
                var response = new { success = true, message = "Usuário ativado com sucesso!" };
                return Ok(response);
            }
            catch (InvalidOperationException)
            {
                return StatusCode(400, "Usuário já esta ativo");
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(401, $"Erro de autorização: {ex.Message}");
            }
            catch (NotFoundException)
            {
                return StatusCode(404, "Usuário não encontrado");
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, $"Tipo de dado inválido: {ex.Message}");
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao ativar o usuário.");
            }
        }

        [HttpPost("{id}/deactivate")]
        [Authorize(Roles = "admin")]
        public IActionResult DeactivateUser(int id)
        {
            try
            {
                _usuarioService.DeactivateUser(id);
                var response = new { success = true, message = "Usuário desativado com sucesso!" };
                return Ok(response);
            }
            catch (InvalidOperationException)
            {
                return StatusCode(400, "Usuário já esta ativo");
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, $"Erro de autorização: {ex.Message}");
            }
            catch (NotFoundException)
            {
                return StatusCode(404, "Usuário não encontrado");
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, $"Tipo de dado inválido: {ex.Message}");
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao ativar o usuário.");
            }
        }

    }
}

