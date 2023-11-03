using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppApi.Controllers
{
    [Route("api/CartDetail")]
    [ApiController]
    public class CartDetailController : ControllerBase
    {
        private ICartDetailRepository _icartDetailRepository;
        public CartDetailController(ICartDetailRepository icartDetailRepository)
        {
            _icartDetailRepository = icartDetailRepository;
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            var a = await _icartDetailRepository.GetAll();
            return Ok(a);
        }
        [HttpPost("add")]
        public async Task<IActionResult> Post(CartDetails obj)
        {
            var a = await _icartDetailRepository.Add(obj);
            return Ok(a);
        }
        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var a = await _icartDetailRepository.GetById(id);
            return Ok(a);
        }

        [HttpGet("getByIdAcout/{id}")]
        public async Task<List<CartDetails>> GetByIdAcout(Guid id)
        {
            var a = await _icartDetailRepository.GetByIdAcout(id);
            return a ;
        }
        [HttpPut("update")]
        public async Task<IActionResult> Put(CartDetails obj)
        {
            var a = await _icartDetailRepository.Update(obj);
            return Ok(a);
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _icartDetailRepository.Delete(id);
            return Ok();
        }
    }
}
