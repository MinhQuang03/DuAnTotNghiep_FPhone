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

        [HttpGet("Ram/{IdRam}")]
        public async Task<IActionResult> GetPhoneByRam(Guid IdRam)
        {
            var a = await _iIPhoneDetailService.GetPhoneByRam(IdRam);
            return Ok(a);
        }

        [HttpGet("Color/{IdColor}")]
        public async Task<IActionResult> GetPhoneByColor(Guid IdColor)
        {
            var a = await _iIPhoneDetailService.GetPhoneByColor(IdColor);
            return Ok(a);
        }

        [HttpGet("ChipCPUs/{IdChipCPU}")]
        public async Task<IActionResult> GetPhoneByChipCPUs(Guid IdChipCPU)
        {
            var a = await _iIPhoneDetailService.GetPhoneByChipCPUs(IdChipCPU);
            return Ok(a);
        }
        [HttpGet("DESC")]
        public async Task<IActionResult> GetPhoneDESC()
        {
            var a = await _iIPhoneDetailService.GetPhoneDESC();
            return Ok(a);
        }
        [HttpGet("ASC")]
        public async Task<IActionResult> GetPhoneASC()
        {
            var a = await _iIPhoneDetailService.GetPhoneASC();
            return Ok(a);
        }
        [HttpGet("5tr")]
        public async Task<IActionResult> GetPhone5tr()
        {
            var a = await _iIPhoneDetailService.GetPhone5tr();
            return Ok(a);
        }
        [HttpGet("10tr")]
        public async Task<IActionResult> GetPhone10tr()
        {
            var a = await _iIPhoneDetailService.GetPhone10tr();
            return Ok(a);
        }
        [HttpGet("15tr")]
        public async Task<IActionResult> GetPhone15tr()
        {
            var a = await _iIPhoneDetailService.GetPhone15tr();
            return Ok(a);
        }
        [HttpGet("25tr")]
        public async Task<IActionResult> GetPhone25tr()
        {
            var a = await _iIPhoneDetailService.GetPhone25tr();
            return Ok(a);
        }
        [HttpGet("50tr")]
        public async Task<IActionResult> GetPhone50tr()
        {
            var a = await _iIPhoneDetailService.GetPhone50tr();
            return Ok(a);
        }
    }
}
