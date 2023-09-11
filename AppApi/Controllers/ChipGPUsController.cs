using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChipGPUsController : ControllerBase
    {
        private IChipGPURepository _chipGPURepository;
        public ChipGPUsController(IChipGPURepository chipGPURepository)
        {
            _chipGPURepository = chipGPURepository;
        }
        // GET: api/<ChipGPUsController>
        [HttpGet("get")]
        public async Task<IActionResult> GetAll()
        {
            var a = await _chipGPURepository.GetAll();
            return Ok(a);
        }

        // GET api/<ChipGPUsController>/5
        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var a = await _chipGPURepository.GetById(id);
            return Ok(a);
        }

        // POST api/<ChipGPUsController>
        [HttpPost("add")]
        public async Task<IActionResult> Post(ChipGPUs obj)
        {
            var a = await _chipGPURepository.Add(obj);
            return Ok(a);
        }

        // PUT api/<ChipGPUsController>/5
        [HttpPut("update")]
        public async Task<IActionResult> Put(ChipGPUs obj)
        {
            var a = await _chipGPURepository.Update(obj);
            return Ok(a);
        }

        // DELETE api/<ChipGPUsController>/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _chipGPURepository.Delete(id);
            return Ok();
        }
    }
}
