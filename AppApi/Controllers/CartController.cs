using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppApi.Controllers
{
    [Route("api/Cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private IcartRepository _icartRepository;
        public CartController(IcartRepository icartRepository)
        {
            _icartRepository = icartRepository;
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {

            var a = await _icartRepository.GetAll();
            return Ok(a);
        }
        [HttpGet("getById/{id}")]
        public async Task<Cart> GetById(Guid id)
        {
            var a = await _icartRepository.GetById(id);
            return a;
        }
        [HttpPost("add")]
        public async Task<IActionResult> Post(Cart obj)
        {
            var a = await _icartRepository.Add(obj);
            return Ok(a);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _icartRepository.Delete(id);
            return Ok();
        }
    }
}
