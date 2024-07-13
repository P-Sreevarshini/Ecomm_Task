using ECommerceApp.Models;
using ECommerceApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace ECommerceApp.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthenticationController> _logger;
        private readonly ApplicationDbContext _context;

        public AuthenticationController(IAuthService authService, ILogger<AuthenticationController> logger, ApplicationDbContext context)
        {
            _authService = authService;
            _logger = logger;
            _context = context;
        }


        // [HttpPost]
        // [Route("login")]
        // public async Task<IActionResult> Login(LoginModel model)
        // {
        //     try
        //     {
        //         if (!ModelState.IsValid)
        //             return BadRequest(new { Status = "Error", Message = "Invalid Payload" });
        //         var (status, message) = await _authService.Login(model);
        //         if (status == 0)
        //             return BadRequest(new { Status = "Error", Message = message });
        //         return Ok(new { Status = "Success", token = message });
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex.Message);
        //         return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //     }
        // }

        [HttpPost]
        [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { Status = "Error", Message = "Invalid Payload" });

                var (status, token, email, userId, userRole,userName) = await _authService.Login(model);

                if (status == 0)
                    return BadRequest(new { Status = "Error", Message = token });

                return Ok(new { Status = "Success", Token = token, Email = email, UserId = userId, UserRole = userRole, Username = userName });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegistrationModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { Status = "Error", Message = "Invalid Payload" });
                
                if (model.UserRole == "Admin" && !model.Email.EndsWith("@admin.com", StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest(new { Status = "Error", Message = "Admin email must end with '@admin.com'" });
                }

                if (model.UserRole == "Admin" || model.UserRole == "Customer")
                {
                    var (status, message) = await _authService.Registeration(model, model.UserRole);
                    if (status == 0)
                    {
                        return BadRequest(new { Status = "Error", Message = message });
                    }
                    var user = new User
                    {
                        Username = model.Username,
                        Password = model.Password,
                        Email = model.Email,
                        MobileNumber = model.MobileNumber,
                        UserRole = model.UserRole,
                    };
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                    return Ok(new { Status = "Success", Message = message });
                }
                else
                {
                    return BadRequest(new { Status = "Error", Message = "Invalid user role" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
