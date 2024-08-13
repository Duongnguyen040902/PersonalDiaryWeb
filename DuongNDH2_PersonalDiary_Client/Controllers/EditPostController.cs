using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using DuongNDH2_PersonalDiary_Client.Models.Dto;
using DuongNDH2_PersonalDiaryClient.Models.ViewModel;

namespace DuongNDH2_PersonalDiaryClient.Controllers
{
    public class EditPostController : Controller
    {
        private readonly HttpClient _httpClient;

        public EditPostController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7062/api/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        [HttpGet]
        public async Task<IActionResult> EditPost(int postId, string origin)
        {
            var token = HttpContext.Session.GetString("Token");
            if (token == null)
            {
                return Unauthorized("Token is missing.");
            }
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"Posts/{postId}");
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("Login", "User");
                }
                else
                {
                    return NotFound("Post not found.");
                }               
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var post = JsonConvert.DeserializeObject<PostEditResponse>(responseContent);

            var viewModel = new EditPostViewModel
            {
                PostEditResponse = post,
                UpdatePostRequest = new UpdatePostRequest
                {
                    Content = post.Content,
                    IsPublic = post.StatusPost == "Public"
                },
                ReturnUrl = origin
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(EditPostViewModel viewModel)
        {
            var token = HttpContext.Session.GetString("Token");
            if (token == null)
            {
                return RedirectToAction("Login", "User");
            }
            var updatePostRequest = viewModel.UpdatePostRequest;
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(updatePostRequest.Content), nameof(updatePostRequest.Content));
            content.Add(new StringContent(updatePostRequest.IsPublic.ToString()), nameof(updatePostRequest.IsPublic));

            if (updatePostRequest.PhotoUrls != null)
            {
                foreach (var photo in updatePostRequest.PhotoUrls)
                {
                    var fileContent = new StreamContent(photo.OpenReadStream());
                    fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                    {
                        Name = "PhotoUrls",
                        FileName = photo.FileName
                    };
                    content.Add(fileContent);
                }
            }

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PutAsync($"Posts/{updatePostRequest.postId}", content);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("Login", "User");
                }
                else
                {
                    return BadRequest("Update failed.");
                }
            }
            if (viewModel.ReturnUrl == "UserProfile")
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                return RedirectToAction("UserProfile", "User", new { userId });
            }

            return RedirectToAction("Index", "Home");
        }


    }
}
