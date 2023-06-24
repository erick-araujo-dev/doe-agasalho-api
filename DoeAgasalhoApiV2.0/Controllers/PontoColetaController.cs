﻿using DoeAgasalhoApiV2._0.Entities;
using DoeAgasalhoApiV2._0.Models;
using DoeAgasalhoApiV2._0.Services;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("create")]
        public ActionResult<PontoColeta> Create(NovoPontoColetaModel novoPontoColeta)
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
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao adicionar o ponto de coleta.");
            }
        }

       
    }
}