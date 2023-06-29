using DoeAgasalhoApiV2._0.Exceptions;
using DoeAgasalhoApiV2._0.Models.Custom_Models;
using DoeAgasalhoApiV2._0.Models.CustomModels;
using DoeAgasalhoApiV2._0.Models.Entities;
using DoeAgasalhoApiV2._0.Services;
using DoeAgasalhoApiV2._0.Services.Interface;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DoeAgasalhoApiV2._0.Controllers
{
    [Route("api/produtos")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoService _produtoService;
        private readonly IUtilsService _utilsService;


        public ProdutoController(IProdutoService produtoService, IUtilsService utilsService)
        {
            _produtoService = produtoService;
            _utilsService = utilsService;
        }
        // GET: api/<ProdutoController>
        [HttpGet("all")]
        public IActionResult GetAll()
        {
            try
            {
                var products = _produtoService.GetProdutosByPontoColeta();
                return Ok(products);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, $"Erro: {ex.Message}");
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao obter todos os usuários.");
            }
        }

        // GET api/<ProdutoController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ProdutoController>
        [HttpPost("create")]
        public ActionResult<ProdutoModel> Create(ProdutoCreateModel product)
        {
            try
            {
                var newProduct = _produtoService.CreateProduct(product);
                return Ok(newProduct);
            }
            catch(UnauthorizedAccessException ex)
            {
                return StatusCode(403, $"Erro: {ex.Message}");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, $"Tipo de dado inválido: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro: {ex.Message}");
            }

        }

        [HttpPut("{id}/updateproduct")]
        public ActionResult<ProdutoModel> Update(int id, UpdateProdutoModel product)
        {
            try
            {
                var updateProduct = _produtoService.UpdateProduct(id, product);
                return Ok(updateProduct);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, $"Tipo de dado inválido: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro: {ex.Message}");
            }
        }

        // DELETE api/<ProdutoController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
