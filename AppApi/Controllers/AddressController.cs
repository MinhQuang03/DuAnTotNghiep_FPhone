using AppData.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private IAddressRepository _addressRepository;

        public AddressController(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        [HttpGet("get-address/{Id}")]
        public async Task<IActionResult> GetUserByAccount(Guid Id)
        {
            var address = await _addressRepository.GetAddress(Id);
            return Ok(address);
        }   
    }
}
