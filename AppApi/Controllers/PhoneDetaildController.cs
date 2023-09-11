using AppData.IRepositories;
using AppData.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhoneDetaildController : ControllerBase
    {
        private IPhoneDetaildRepository _phoneDetaildRepository;
        public PhoneDetaildController(IPhoneDetaildRepository phoneDetaildRepository) 
        {
            _phoneDetaildRepository = phoneDetaildRepository;
        }
        // GET: api/<PhoneDetaildController>
        [HttpGet("get")]
        public async Task<IActionResult> GetAll()
        {
            var a = await _phoneDetaildRepository.GetAll();
            return Ok(a);
        }

        // GET api/<PhoneDetaildController>/5
        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var a = await _phoneDetaildRepository.GetById(id);
            return Ok(a);
        }

        // POST api/<PhoneDetaildController>
        [HttpPost("add")]
        public async Task<IActionResult> Post(PhoneDetaild obj)
        {
            var a = await _phoneDetaildRepository.Add(obj);
            return Ok(a);
        }

        // PUT api/<PhoneDetaildController>/5
        [HttpPut("update")]
        public async Task<IActionResult> Put(PhoneDetaild obj)
        {
            var a = await _phoneDetaildRepository.Update(obj);
            return Ok(a);
        }

    }
}
