using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChipCPUsController : ControllerBase
    {
        private IChipCPURepository _chipCPURepository;
        public ChipCPUsController(IChipCPURepository chipCPURepository)
        {
            _chipCPURepository = chipCPURepository;
        }
        // GET: api/<ChipCPUsController>
        [HttpGet("get")]
        public async Task<IActionResult> GetAll()
        {
            var a = await _chipCPURepository.GetAll();
            return Ok(a);
        }

        // GET api/<ChipCPUsController>/5
        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var a = await _chipCPURepository.GetById(id);
            return Ok(a);
        }

        // POST api/<ChipCPUsController>
        [HttpPost("add")]
        public async Task<IActionResult> Post(ChipCPUs obj)
        {
            var a = await _chipCPURepository.Add(obj);
            return Ok(a);
        }

        // PUT api/<ChipCPUsController>/5
        [HttpPut("update")]
        public async Task<IActionResult> Put(ChipCPUs obj)
        {
            var a = await _chipCPURepository.Update(obj);
            return Ok(a);
        }

        // DELETE api/<ChipCPUsController>/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _chipCPURepository.Delete(id);
            return Ok();
        }
    }
}
