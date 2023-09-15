using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppApi.Controllers
{
    [Route("api/material")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private IMaterialRepository _materialRepository;
        public MaterialController(IMaterialRepository materialRepository)
        {
            _materialRepository = materialRepository;
        }
        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {

            var a = await _materialRepository.GetAll();
            return Ok(a);
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var a = await _materialRepository.GetById(id);
            return Ok(a);
        }

        // POST api/<ProductionCompanyController>
        [HttpPost("add")]
        public async Task<IActionResult> Post(Material obj)
        {
            var a = await _materialRepository.Add(obj);
            return Ok(a);
        }

        // PUT api/<ProductionCompanyController>/5
        [HttpPut("update")]
        public async Task<IActionResult> Put(Material obj)
        {
            var a = await _materialRepository.Update(obj);
            return Ok(a);
        }

        // DELETE api/<ProductionCompanyController>/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _materialRepository.Delete(id);
            return Ok();
        }
    }
}
