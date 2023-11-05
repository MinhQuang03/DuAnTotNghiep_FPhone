using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhoneController : ControllerBase
    {
        private IPhoneRepository _phoneRepository;
        public PhoneController(IPhoneRepository phoneRepository)
        {
            _phoneRepository = phoneRepository;
        }
        // GET: api/<PhoneController>
        [HttpGet("get")]
        public async Task<IActionResult> GetAll()
        {
            var a = await _phoneRepository.GetAll();
            return Ok(a);
        }

        // GET api/<PhoneController>/5
        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var a = await _phoneRepository.GetById(id);
            return Ok(a);
        }

        // POST api/<PhoneController>
        [HttpPost("add")]
        public async Task<IActionResult> Post(Phone obj)
        {
            var a = await _phoneRepository.Add(obj);
            return Ok(a);
        }

        // PUT api/<PhoneController>/5
        [HttpPut("update")]
        public async Task<IActionResult> Put(Phone obj)
        {
            var a = await _phoneRepository.Update(obj);
            return Ok(a);
        }

        // DELETE api/<PhoneController>/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _phoneRepository.Delete(id);
            return Ok();
        }

     
    }
}
