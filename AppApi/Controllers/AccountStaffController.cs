using AppData.IRepositories;
using AppData.Models;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountStaffController : ControllerBase
    {
        private readonly IAccountStaffRepository _staffRepository;
        public AccountStaffController(IAccountStaffRepository staffRepository)
        {
            _staffRepository = staffRepository;
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpModel signUpModel)
        {
            var result = await _staffRepository.SignUpAsync(signUpModel);
            if (result)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SignInModel signInModel)
        {
            var result = await _staffRepository.SignInAsync(signInModel);

            if (string.IsNullOrEmpty(result))
            {
                return BadRequest(new ApiResponse()
                {
                    Success = false,
                    Message = "Invalid username/password",
                    Data = signInModel
                });
            }

            return Ok(result);
        }

    }
}
