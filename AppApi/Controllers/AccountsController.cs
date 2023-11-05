using AppData.IRepositories;
using AppData.Models;
using AppData.Utilities;
using AppData.ViewModels.Accounts;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IAccountsRepository _accountsRepository;
    private readonly IUserRepository _userRepository;

    public AccountsController(IAccountsRepository accountsRepository,IUserRepository userRepository)
    {
        _accountsRepository = accountsRepository;   
        _userRepository = userRepository;
    }

    [HttpPost("SignUp/Admin/")]

    public async Task<IActionResult> SignUp(AdSignUpViewModel signUpModel)
    {
        var result = _accountsRepository.SignUpAdmin(signUpModel);
        if (!result.IsCompletedSuccessfully) return Ok(result.Result);
        return BadRequest(result.Result);
    }
    [HttpPost("SignUp/Client/")]

    public async Task<IActionResult> SignUp(ClAccountsViewModel signUpModel)
    {
        var result = _accountsRepository.SignUpCl(signUpModel);
        if (!result.IsCompletedSuccessfully) return Ok(result.Result);
        return BadRequest(result.Result);
    }

    [HttpPost("Login/")]
    public async Task<IActionResult> Login(LoginModel model)
    {
        var clLogin = await _accountsRepository.ClLogin(model);
        var adLogin = await _accountsRepository.AdLogin(model);
        if (clLogin.Valid && adLogin.Valid) return BadRequest("Nếu bạn gặp lỗi này vui lòng liên hệ quản trị viên");
        if (adLogin.Valid)
        {
            return Ok(adLogin);
        }

        if (clLogin.Valid)
        {
            return Ok(clLogin);
        }

        return BadRequest("Sai tài khoản hoặc mật khẩu. Vui lòng thử lại");
    }

    //[HttpPost("Login/Client/")]
    //public async Task<IActionResult> Login(LoginModel model)
    //{
    //    var result = await _accountsRepository.ClLogin(model);
    //    return Ok(result);
    //}

    [HttpGet("LoginWithToken/{token}")]
    public IActionResult LoginWithToken(string token)
    {
        return null;
    }

     [HttpGet("get-all-staff")]
   
    public async Task<List<ApplicationUser>> GetAll()
    {
        var result = await _accountsRepository.GetAllAsync();
        return result;
    }

    [HttpGet("get-all-user")]
    public async Task<List<Account>> GetAllUser()   
    {
        var result = await _userRepository.GetAllAsync();
        return result;
    }

    [HttpGet("get-user/{id}")]
    public async Task<Account> GetUserById(Guid id)
    {   
        var result = await _userRepository.GetById(id);
        return result;
    }

}