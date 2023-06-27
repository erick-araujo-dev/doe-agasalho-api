using DoeAgasalhoApiV2._0.Exceptions;
using DoeAgasalhoApiV2._0.Models.CustomModels;
using DoeAgasalhoApiV2._0.Models.Entities;
using DoeAgasalhoApiV2._0.Services;
using DoeAgasalhoApiV2._0.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace DoeAgasalhoApiV2._0.Controllers
{
    [Route("api/pontocoleta")]
    [ApiController]
    public class PontoColetaController : ControllerBase
    {
        private readonly IPontoColetaService _pontoColetaService;

        public PontoColetaController(IPontoColetaService pontoColetaService)
        {
            _pontoColetaService = pontoColetaService;
        }

        [HttpGet("all")]
        public IActionResult Get()
        {
            try
            {
                var collectPoints = _pontoColetaService.GetAllCollectPoint();
                return Ok(collectPoints);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao obter todos os usuários.");
            }
        }

        [HttpGet("active")]
        public IActionResult GetActive()
        {
            try
            {
                var users = _pontoColetaService.GetActivateCollectPoint();
                return Ok(users);
            }

            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao obter os usuários ativos.");
            }
        }

        [HttpGet("inactive")]
        public IActionResult GetInactive()
        {
            try
            {
                var users = _pontoColetaService.GetInactiveCollectPoint();
                return Ok(users);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao obter os usuários inativos.");
            }
        }



        [HttpPost("create")]
        public ActionResult<PontoColetaModel> Create(NovoPontoColetaModel novoPontoColeta)
        {
            try
            {
                var pontoColeta = _pontoColetaService.CreateCollectPoint(novoPontoColeta);
                return Ok(pontoColeta);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, $"Tipo de dado inválido: {ex.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPut("{id}/updateusername")]
        public ActionResult<PontoColetaModel> Update(int id, UpdateUsernameModel updatePontoColeta)
        {
            try
            {
                var pontoColeta = _pontoColetaService.UpdateUsername(id, updatePontoColeta);
                return Ok(pontoColeta);

            } catch(NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BusinessOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}/activate")]
        public IActionResult Activate(int id)
        {
            try
            {
                _pontoColetaService.ActivateCollectPoint(id);
                var response = new { success = true, message = "Ponto de coleta ativado com sucesso!" };
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao ativar o usuário.");
            }
        }

        [HttpPost("{id}/deactivate")]
        public IActionResult Deactivate(int id)
        {
            try
            {
                _pontoColetaService.DeactivateCollectPoint(id);
                var response = new { success = true, message = "Ponto de coleta desativado com sucesso!" };
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);  
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao ativar o usuário.");
            }
        }
    }
}
