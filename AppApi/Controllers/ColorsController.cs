using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorsController : ControllerBase
    {
        private IColorRepository _colorRepository;
        public ColorsController(IColorRepository colorRepository)
        {
            _colorRepository = colorRepository;
        }
        // GET: api/<ColorsController>
        [HttpGet("get")]
        public async Task<IActionResult> GetAll()
        {
            var a = await _colorRepository.GetAll();
            return Ok(a);
        }

        // GET api/<ColorsController>/5
        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var a = await _colorRepository.GetById(id);
            return Ok(a);
        }

        // POST api/<ColorsController>
        [HttpPost("add")]
        public async Task<IActionResult> Post(Color? obj)
        {
            var a = await _colorRepository.Add(obj);
            return Ok(a);
        }

        // PUT api/<ColorsController>/5
        [HttpPut("update")]
        public async Task<IActionResult> Put(Color obj)
        {
            var a = await _colorRepository.Update(obj);
            return Ok(a);
        }

        // DELETE api/<ColorsController>/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _colorRepository.Delete(id);
            return Ok();
        }
    }
}
