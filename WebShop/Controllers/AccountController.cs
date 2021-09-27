using Database.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using WebShop.ViewModels.Account;

namespace WebShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Registers a new user
        /// </summary>
        /// <param name="requst"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest requst)
        {
            User user = new()
            {
                Email = requst.Email,
                UserName = requst.Email,
                Firstname = requst.Firstname,
                Secondname = requst.Secondname
            };

            var result = await _userManager.CreateAsync(user, requst.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return Ok();
            }
            else
            {
                ModelStateDictionary errors = new();
                foreach (var error in result.Errors)
                {
                    errors.AddModelError(error.Code, error.Description);
                }
                return ValidationProblem(errors);
            }

        }

        /// <summary>
        /// Authorizes the user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            HttpContext.Response.ContentType = "application/json";
            var result =
                await _signInManager.PasswordSignInAsync(request.Email, request.Password, request.RememberMe, false);
            if (result.Succeeded)
            {

                return Ok(request.Email);
            }
            else
            {
                ModelStateDictionary errors = new();
                errors.AddModelError("Login password", "Wrong login or password");
                return ValidationProblem(errors);
            }
        }

        /// <summary>
        /// Checks if the user is sign in
        /// </summary>
        /// <returns></returns>
        [HttpGet("IsSignIn")]
        public IActionResult IsSignIn()
        {
            if (_signInManager.IsSignedIn(User) == true)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Removes user's coockies 
        /// </summary>
        /// <returns></returns>
        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

    }
}
