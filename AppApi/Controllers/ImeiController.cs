using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImeiController : ControllerBase
    {
        private IImeiRepository _imeiRepository { get; set; }
        public ImeiController(IImeiRepository imeiRepository)
        {
            _imeiRepository = imeiRepository;
        }
        // GET: api/<ImeiController>
        [HttpGet("get")]
        public async Task<IActionResult> GetAll()
        {
            var a = await _imeiRepository.GetAll();
            return Ok(a);
        }

        // GET api/<ImeiController>/5
        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var a = await _imeiRepository.GetById(id);
            return Ok(a);
        }

        // POST api/<ImeiController>
        [HttpPost("add")]
        public async Task<IActionResult> Post(Imei obj)
        {
            var a = await _imeiRepository.Add(obj);
            return Ok(a);
        }

        // PUT api/<ImeiController>/5
        [HttpPut("update")]
        public async Task<IActionResult> Put(Imei obj)
        {
            var a = await _imeiRepository.Update(obj);
            return Ok(a);
        }


        // DELETE api/<ImeiController>/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _imeiRepository.Delete(id);
            return Ok();
        }
    }
}
