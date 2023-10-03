using AppData.IRepositories;
using AppData.IServices;
using AppData.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhoneDetaildController : ControllerBase
    {
        private IPhoneDetailService _iIPhoneDetailService;
        public PhoneDetaildController(IPhoneDetailService iIPhoneDetailService) 
        {
            _iIPhoneDetailService = iIPhoneDetailService;
        }
        // GET: api/<PhoneDetaildController>
        [HttpGet("get")]
        public async Task<List<PhoneDetaild>> GetAll()
        {
            var a = await _iIPhoneDetailService.GetPhoneDetailds();
            return a;
        }
        [HttpGet("get-detail/{id}")]
        public async Task<List<PhoneDetaild>> GetAll(Guid id)
        {
            var a = await _iIPhoneDetailService.GetPhoneDetailds(id);
            return a;
        }

        // GET api/<PhoneDetaildController>/5
        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var a = await _iIPhoneDetailService.GetById(id);
            return Ok(a);
        }

        // POST api/<PhoneDetaildController>
        [HttpPost("add")]
        public async Task<IActionResult> Post(PhoneDetaild obj)
        {
            var a = await _iIPhoneDetailService.Add(obj);
            return Ok(a);
        }

        // PUT api/<PhoneDetaildController>/5
        [HttpPut("update")]
        public async Task<IActionResult> Put(PhoneDetaild obj)
        {
            var a = await _iIPhoneDetailService.Update(obj);
            return Ok(a);
        }

    }
}
