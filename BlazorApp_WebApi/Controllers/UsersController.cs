using BlazorApp_WebApi.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Models;
using Newtonsoft.Json;
using Services;

namespace BlazorApp_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private UserService _userService;
        private IHubContext<ChatHub> _chathub;
        public UsersController(UserService userService, IHubContext<ChatHub> chathub)
        {
            _userService = userService;
            _chathub = chathub;
        }

        [Route("GetAllUsers")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {

            try
            {
                var Result = await _userService.GetAllUsersAsync();

                if (Result == null || !Result.Any())
                {
                    return NotFound("No users found.");
                }
                return Ok(Result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("GetUserByID")]
        [HttpGet]
        public async Task<IActionResult> GetUserByID(int ID)
        {
            try
            {
                var Result = await _userService.GetUserByIdAsync(ID);

                if (Result == null)
                {
                    return NotFound("No users found.");
                }
                return Ok(Result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [Route("InsertUser")]
        [HttpPost]
        public async Task<IActionResult> InsertUser([FromBody] Users _Users)
        {
            try
            {
                int? Result = await _userService.CreateUserAsync(_Users);

                if (Result != null && Result != 0)
                {
                    await SignalRCall();

                    return Ok("Inserted UserID: " + Result);
                }
                else
                {
                    return BadRequest("No users inserted.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Route("UpdateUser")]
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] Users _Users)
        {
            try
            {
                var Result = await _userService.UpdateUserAsync(_Users);

                if (Result)
                {
                    await SignalRCall();
                    return Ok("User Updated Successfully.");
                }
                else
                {
                    return BadRequest("No Users Updated.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [Route("DeleteUser")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int ID)
        {
            try
            {
                var Result = await _userService.DeleteUserAsync(ID);

                if (Result)
                {
                    await SignalRCall();
                    return Ok("User Deleted Successfully.");
                }
                else
                {
                    return BadRequest("No Users Deleted.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private async Task SignalRCall()
        {
            var Users = await _userService.GetAllUsersAsync();
            var serializedResult = JsonConvert.SerializeObject(Users.ToList());
            await _chathub.Clients.All.SendAsync("ReceiveAllUsers", serializedResult);
        }
    }
}
