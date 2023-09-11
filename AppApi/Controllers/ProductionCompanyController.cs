using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionCompanyController : ControllerBase
    {
        private IProductionCompanyRepository _productionCompanyRepository;
        public ProductionCompanyController(IProductionCompanyRepository productionCompanyRepository)
        {
            _productionCompanyRepository = productionCompanyRepository;
        }
        // GET: api/<ProductionCompanyController>
        [HttpGet("get")]
        public async Task<IActionResult> GetAll()
        {
            var a = await _productionCompanyRepository.GetAll();
            return Ok(a);
        }

        // GET api/<ProductionCompanyController>/5
        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var a = await _productionCompanyRepository.GetById(id);
            return Ok(a);
        }

        // POST api/<ProductionCompanyController>
        [HttpPost("add")]
        public async Task<IActionResult> Post(ProductionCompany obj)
        {
            var a = await _productionCompanyRepository.Add(obj);
            return Ok(a);
        }

        // PUT api/<ProductionCompanyController>/5
        [HttpPut("update")]
        public async Task<IActionResult> Put(ProductionCompany obj)
        {
            var a = await _productionCompanyRepository.Update(obj);
            return Ok(a);
        }

        // DELETE api/<ProductionCompanyController>/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _productionCompanyRepository.Delete(id);
            return Ok();
        }
    }
}
