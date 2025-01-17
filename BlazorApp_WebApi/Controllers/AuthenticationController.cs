using BlazorApp_WebApi.JWT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace BlazorApp_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private GenerateToken _generatetoken;
        public AuthenticationController(GenerateToken generatetoken)
        {
            _generatetoken = generatetoken;
        }

        [AllowAnonymous]
        [HttpPost("AuthToken")]
        public IActionResult AuthToken([FromBody] LoginRequest request)
        {
            /* Simple validation for demo
             * It should be like :
             * First check wheather the modal is valid or not
             * Consume Loginservice --> LoginRepository --> DAL --> stored procedure
             * Get user password from database which is encrypted using SHA-256 salt,keysize and No of Iterations
             * The entered password should be encryped
             * Both password be comparied
             * 
            */
            if (request.Username == "admin" && request.Password == "password") 
            {

                Users _user = new Users();
                string token = _generatetoken.GenerateJwtToken(_user);
                return Ok(new { Token = token });
            }
            return Unauthorized();
        }

    }
}
