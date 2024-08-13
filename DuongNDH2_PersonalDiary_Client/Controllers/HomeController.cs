using DuongNDH2_PersonalDiary_Client.Models.Dto;
using DuongNDH2_PersonalDiary_Client.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace DuongNDH2_PersonalDiary_Client.Controllers { 

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;

        public HomeController(ILogger<HomeController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7062/api/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IActionResult> Index()
        {    
            var viewModel = new IndexViewModel();

            var token = HttpContext.Session.GetInt32("Token");
            var userId = HttpContext.Session.GetInt32("UserId");
            try
            {
                if(userId == null)
                {
                    viewModel.User = new UserDto
                    {
                        UserId = -1,
                        UserName = "User",
                        ProfilePictureUrl = "/images/Avatar_default.jpg"
                    };

                    // Fetch posts
                    var postsResponse = await _httpClient.GetAsync("posts");
                    if (postsResponse.IsSuccessStatusCode)
                    {
                        var postsContent = await postsResponse.Content.ReadAsStringAsync(); 
                        var posts = JsonConvert.DeserializeObject<List<PostDto>>(postsContent);
                        // Check if the current user has liked each post
                        foreach (var post in posts)
                        {
                            post.IsLikedByCurrentUser = false;
                        }
                        viewModel.Posts = posts;
                    }
                    else
                    {
                        return NotFound("Post not found");
                    }
                }
                else
                {
                    // Fetch user details
                    var userResponse = await _httpClient.GetAsync($"Users/{userId}");
                    if (userResponse.IsSuccessStatusCode)
                    {
                        var userContent = await userResponse.Content.ReadAsStringAsync();
                        viewModel.User = JsonConvert.DeserializeObject<UserDto>(userContent);
                    }
                    else
                    {
                        return NotFound("User not found");
                    }

                    // Fetch posts
                    var postsResponse = await _httpClient.GetAsync("posts");
                    if (postsResponse.IsSuccessStatusCode)
                    {
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

                        }

                        viewModel.Posts = posts;

                    }
                    else
                    {
                        return NotFound("Post not found");
                    }
                }
                
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
            }

            return View(viewModel);
        }
    }
}
