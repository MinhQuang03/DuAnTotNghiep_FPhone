using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppApi.Controllers
{
    [Route("api/Rom")]
    [ApiController]
    public class RomController : ControllerBase
    {
        private IRomRepository _romRepository;
        public RomController(IRomRepository RomController)
        {
            _romRepository = RomController;
        }
        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {

            var a = await _romRepository.GetAll();
            return Ok(a);
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var a = await _romRepository.GetById(id);
            return Ok(a);
        }

        // POST api/<ProductionCompanyController>
        [HttpPost("add")]
        public async Task<IActionResult> Post(Rom obj)
        {
            var a = await _romRepository.Add(obj);
            return Ok(a);
        }

        // PUT api/<ProductionCompanyController>/5
        [HttpPut("update")]
        public async Task<IActionResult> Put(Rom obj)
        {
            var a = await _romRepository.Update(obj);
            return Ok(a);
        }

        // DELETE api/<ProductionCompanyController>/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _romRepository.Delete(id);
            return Ok();
        }
    }
}
