using DuongNDH2_PersonalDiary_Client.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json;
using DuongNDH2_PersonalDiary_Client.Models.Dto;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;

namespace DuongNDH2_PersonalDiaryClient.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly HttpClient _httpClient;

        public UserController(ILogger<UserController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7062/api/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            if (ModelState.IsValid)
            {
                // Set default profile picture URL if not provided by the client
                if (string.IsNullOrEmpty(model.ProfilePictureUrl))
                {
                    model.ProfilePictureUrl = "/images/Avatar_default.jpg";
                }
                var jsonContent = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("Users/register", content);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    // Registration successful
                    return RedirectToAction("Login", "User");
                }
                else
                {
                    // Handle error
                    ModelState.AddModelError(string.Empty, "Registration failed.");
                }
            }
            return View(model);
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            if (ModelState.IsValid)
            {
                var jsonContent = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync("Users/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(responseContent);

                    HttpContext.Session.SetString("Token", loginResponse.Token);

                    // Giải mã token để lấy UserId
                    var handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadToken(loginResponse.Token) as JwtSecurityToken;
                    var userIdClaim = jsonToken?.Claims.First(claim => claim.Type == "UserId");
                    var role = jsonToken?.Claims.First(claim => claim.Type == "role").Value;
                    var nameClaim = jsonToken?.Claims.First(claim => claim.Type == "unique_name");
                    var profilePicClaim = jsonToken?.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/uri");
                    if (role == "User")
                    {
                        HttpContext.Session.SetString("UserName", nameClaim?.Value);
                        HttpContext.Session.SetString("ProfilePictureUrl", profilePicClaim?.Value);
                        HttpContext.Session.SetInt32("UserId", int.Parse(userIdClaim.Value));
                    }
                    else
                    {
                        return RedirectToAction("Login", "User");
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email hoặc mật khẩu không đúng");
                }

            }
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Token");
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("Login", "User");
        }

        public async Task<IActionResult> UserProfile(int userId)
        {
            var token = HttpContext.Session.GetString("Token");
            var viewModel = new IndexViewModel();         
            try
            {
                // Fetch user details
                var userResponse = await _httpClient.GetAsync($"Users/{userId}");
                if (!userResponse.IsSuccessStatusCode)
                {
                    if(userResponse == null)
                    {
                        return NotFound("User not found.");
                    }                              
                }
                var userContent = await userResponse.Content.ReadAsStringAsync();
                viewModel.User = JsonConvert.DeserializeObject<UserDto>(userContent);

                // Fetch posts
                if (token == null)
                {
                    return RedirectToAction("Login", "User");
                }
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var postsResponse = await _httpClient.GetAsync($"Posts/user/{userId}");
                if (!postsResponse.IsSuccessStatusCode)
                {
                    if (postsResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                         return RedirectToAction("Login", "User");
                    }
                    else
                    {
                        return NotFound("Post not found.");
                    }
                }
                var postsContent = await postsResponse.Content.ReadAsStringAsync();
                var posts = JsonConvert.DeserializeObject<List<PostDto>>(postsContent);
                // Check if the current user has liked each post
                foreach (var post in posts)
                {
                    var likedResponse = await _httpClient.GetAsync($"PostLikes/{userId}/{post.PostId}");
                    if (likedResponse.IsSuccessStatusCode)
                    {
                        var likedContent = await likedResponse.Content.ReadAsStringAsync();
                        var liked = JsonConvert.DeserializeObject<bool>(likedContent);
                        post.IsLikedByCurrentUser = liked;
                    }
                    else
                    {
                        // Handle error when fetching like status
                        _logger.LogError($"Failed to retrieve like status for post {post.PostId}. Status code: {likedResponse.StatusCode}");
                    }
                }

                viewModel.Posts = posts;
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                _logger.LogError($"An error occurred while fetching data: {ex.Message}");
                // Optionally redirect to an error page or display a generic error message
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
            }

            return View(viewModel);
        }
    }
}
