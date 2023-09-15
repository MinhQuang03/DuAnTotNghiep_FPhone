using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppApi.Controllers
{
    [Route("api/Sim")]
    [ApiController]
    public class SimController : ControllerBase
    {
        private ISimRepository _simRepository;
        public SimController(ISimRepository simRepository)
        {
            _simRepository = simRepository;
        }


        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {

            var a = await _simRepository.GetAll();
            return Ok(a);
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var a = await _simRepository.GetById(id);
            return Ok(a);
        }

        // POST api/<ProductionCompanyController>
        [HttpPost("add")]
        public async Task<IActionResult> Post(Sim obj)
        {
            var a = await _simRepository.Add(obj);
            return Ok(a);
        }

        // PUT api/<ProductionCompanyController>/5
        [HttpPut("update")]
        public async Task<IActionResult> Put(Sim obj)
        {
            var a = await _simRepository.Update(obj);
            return Ok(a);
        }

        // DELETE api/<ProductionCompanyController>/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _simRepository.Delete(id);
            return Ok();
        }
    }
}
