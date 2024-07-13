// using ECommerceApp.Models;
// using ECommerceApp.Services;
// using Microsoft.AspNetCore.Mvc;
// using System;
// using System.Net.Http;
// using System.Net.Http.Headers;
// using System.Text.Json;
// using System.Threading.Tasks;
// using Microsoft.Extensions.Logging;
// using System.Text;

// namespace ECommerceApp.Controllers
// {
//    public class AuthenticationMvcController : Controller
//     {
//         private readonly IAuthService _authService;
//         private readonly IHttpClientFactory _clientFactory;
//         private readonly ILogger<AuthenticationMvcController> _logger;
//         private readonly string _baseUrl = "http://localhost:5069"; // Base URL of your Web API

//         public AuthenticationMvcController(IHttpClientFactory clientFactory, ILogger<AuthenticationMvcController> logger,IAuthService authService)
//         {
//             _clientFactory = clientFactory;
//             _authService = authService;

//             _logger = logger;
//         }

//         public IActionResult Login()
//         {
//             return View();
//         }

//        public async Task<IActionResult> Login(LoginModel model)
// {
//     try
//     {
//         var client = _clientFactory.CreateClient();
//         client.BaseAddress = new Uri(_baseUrl);
//         client.DefaultRequestHeaders.Accept.Clear();
//         client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

//         var jsonContent = JsonSerializer.Serialize(model);
//         var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
//         _logger.LogInformation("Login Request: " + jsonContent);

//         using (var response = await client.PostAsync("api/login", content))
//         {
//             if (response.IsSuccessStatusCode)
//             {
//                 var responseBody = await response.Content.ReadAsStringAsync();
//                 _logger.LogInformation("Login Response: " + responseBody);

//                 var result = JsonSerializer.Deserialize<LoginResponse>(responseBody);

//                 HttpContext.Session.SetString("Token", result.Token);
//                 HttpContext.Session.SetString("Username", result.Username);
//                 HttpContext.Session.SetString("UserRole", result.UserRole);

//                 return RedirectToAction("Index", "ProductMvc");
//             }
//             else
//             {
//                 var errorResponse = await response.Content.ReadAsStringAsync();
//                 _logger.LogError("Login Failed: " + errorResponse);

//                 ModelState.AddModelError(string.Empty, "Invalid login attempt.");
//                 return View(model);
//             }
//         }
//     }
//     catch (Exception ex)
//     {
//         _logger.LogError("Login Exception: " + ex.ToString());
//         ModelState.AddModelError(string.Empty, "Login failed.");
//         return View(model);
//     }
// }

//     //      [HttpPost]
//     // public async Task<IActionResult> Login(LoginModel model)
//     // {
//     //     try
//     //     {
//     //         var token = await _authService.AuthenticateAsync(model);
//     //         if (token != null)
//     //         {
//     //             HttpContext.Session.SetString("Token", token);
//     //             HttpContext.Session.SetString("Email", model.Email); // Store username for reference
//     //             HttpContext.Session.SetString("UserRole", "Customer"); // Assume role based on authentication logic

//     //             return RedirectToAction("Index", "ProductMvc");
//     //         }
//     //         else
//     //         {
//     //             ModelState.AddModelError(string.Empty, "Invalid login attempt.");
//     //             return View(model);
//     //         }
//     //     }
//     //     catch (Exception ex)
//     //     {
//     //         _logger.LogError("Login Exception: " + ex.Message);
//     //         ModelState.AddModelError(string.Empty, "Login failed.");
//     //         return View(model);
//     //     }
//     // }

//         public IActionResult Register()
//         {
//             return View();
//         }

//         [HttpPost]
//         public async Task<IActionResult> Register(RegistrationModel model)
//         {
//             _logger.LogInformation("Starting Registration Process");
//             try
//             {
//                 var client = _clientFactory.CreateClient();
//                 client.BaseAddress = new Uri(_baseUrl);
//                 client.DefaultRequestHeaders.Accept.Clear();
//                 client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

//                 var content = new StringContent(JsonSerializer.Serialize(model), System.Text.Encoding.UTF8, "application/json");
//                 var response = await client.PostAsync("/api/register", content);

//                 if (response.IsSuccessStatusCode)
//                 {
//                     _logger.LogInformation("Registration Successful");
//                     return RedirectToAction("Login"); // Redirect to login page after successful registration
//                 }
//                 else
//                 {
//                     var responseBody = await response.Content.ReadAsStringAsync();
//                     _logger.LogError("Registration Failed: " + response.ReasonPhrase + " - " + responseBody);
//                     ModelState.AddModelError(string.Empty, "Registration failed.");
//                     return View(model);
//                 }
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError("Registration Exception: " + ex.Message);
//                 ModelState.AddModelError(string.Empty, $"Registration failed: {ex.Message}");
//                 return View(model);
//             }
//         }
//     }
// }
using ECommerceApp.Models;
using ECommerceApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerceApp.Controllers
{
    public class AuthenticationMvcController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<AuthenticationMvcController> _logger;
        private readonly string _baseUrl = "http://localhost:5069"; 

        public AuthenticationMvcController(IHttpClientFactory clientFactory, ILogger<AuthenticationMvcController> logger, IAuthService authService)
        {
            _clientFactory = clientFactory;
            _authService = authService;
            _logger = logger;
        }

        public IActionResult Login()
        {
            return View();
        }

        //                 [HttpPost]
        // public async Task<IActionResult> Login(LoginModel model)
        // {
        //     try
        //     {
        //         var client = _clientFactory.CreateClient();
        //         client.BaseAddress = new Uri(_baseUrl);
        //         client.DefaultRequestHeaders.Accept.Clear();
        //         client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //         var jsonContent = JsonSerializer.Serialize(model);
        //         var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        //         _logger.LogInformation("Login Request: " + jsonContent);

        //         using (var response = await client.PostAsync("api/login", content))
        //         {
        //             var responseBody = await response.Content.ReadAsStringAsync();
        //             _logger.LogInformation("Login Response: " + responseBody);

        //             if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //             {
        //                 var result = JsonSerializer.Deserialize<LoginResponse>(responseBody);

        //                 HttpContext.Session.SetString("Token", result.Token);
        //                 HttpContext.Session.SetString("Username", result.Username);
        //                 HttpContext.Session.SetString("UserRole", result.UserRole);

        //                 return RedirectToAction("Index", "ProductMvc");
        //             }
        //             else
        //             {
        //                 _logger.LogError($"Login Failed. Status Code: {response.StatusCode}, Response: {responseBody}");
        //                 ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        //                 return View(model);
        //             }
        //         }
        //     }
        //     catch (HttpRequestException ex)
        //     {
        //         _logger.LogError("HTTP Request Exception: " + ex.Message);
        //         ModelState.AddModelError(string.Empty, "HTTP Request failed. Please try again later.");
        //         return View(model);
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError("Login Exception: " + ex.Message);
        //         ModelState.AddModelError(string.Empty, "Login failed. Please try again later.");
        //         return View(model);
        //     }
        // }
        [HttpPost]
        // [ValidateAntiForgeryToken]

        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var (status, token, email, userId, userRole, userName) = await _authService.Login(model);

                if (status == 0)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }

                HttpContext.Session.SetString("Token", token);
                HttpContext.Session.SetString("Username", userName);
                HttpContext.Session.SetString("UserRole", userRole);

                return RedirectToAction("Index", "ProductMvc");
            }
            catch (Exception ex)
            {
                _logger.LogError("Login Exception: " + ex.Message);
                ModelState.AddModelError(string.Empty, "Login failed. Please try again later.");
                return View(model);
            }
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationModel model)
        {
            _logger.LogInformation("Starting Registration Process");
            try
            {
                var client = _clientFactory.CreateClient();
                client.BaseAddress = new Uri(_baseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var content = new StringContent(JsonSerializer.Serialize(model), System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync("/api/register", content);

                if (response.IsSuccessStatusCode)
                // if (response.StatusCode == HttpStatusCode.OK)

                {
                    _logger.LogInformation("Registration Successful");
                    return RedirectToAction("Login"); // Redirect to login page after successful registration
                }
                else
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Registration Failed: " + response.ReasonPhrase + " - " + responseBody);
                    ModelState.AddModelError(string.Empty, "Registration failed.");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Registration Exception: " + ex.Message);
                ModelState.AddModelError(string.Empty, $"Registration failed: {ex.Message}");
                return View(model);
            }
        }
    }
}
