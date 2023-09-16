using AppData.IRepositories;
using AppData.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppApi.Controllers
{
    [Route("api/Ram")]
    [ApiController]
    public class RamController : ControllerBase
    {
        private IRamRepository _ramRepository;
        public RamController(IRamRepository RamController)
        {
            _ramRepository = RamController;
        }
        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {

            var a = await _ramRepository.GetAll();
            return Ok(a);
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var a = await _ramRepository.GetById(id);
            return Ok(a);
        }

        // POST api/<ProductionCompanyController>
        [HttpPost("add")]
        public async Task<IActionResult> Post(Ram obj)
        {
            var a = await _ramRepository.Add(obj);
            return Ok(a);
        }

        // PUT api/<ProductionCompanyController>/5
        [HttpPut("update")]
        public async Task<IActionResult> Put(Ram obj)
        {
            var a = await _ramRepository.Update(obj);
            return Ok(a);
        }

        // DELETE api/<ProductionCompanyController>/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _ramRepository.Delete(id);
            return Ok();
        }
    }
}
