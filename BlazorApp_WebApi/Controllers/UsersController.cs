using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace BlazorApp_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private UserService _userService;
        public UsersController(UserService userService)
        {
            _userService = userService;
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

                if (Result == null && Result == 0)
                {
                    return BadRequest("No users inserted.");
                }
                return Ok("Inserted UserID: " + Result);
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

                if (!Result)
                {
                    return BadRequest("No Users Updated.");
                }
                return Ok("User Updated Successfully.");
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

                if (!Result)
                {
                    return BadRequest("No Users Deleted.");
                }
                return Ok("User Deleted Successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
