using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QueueManagmentBL.Interfaces;
using QueueManagmentDB.EF.Models;
using QueueManagmentEntites;

namespace QueueManagmentApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly IUserBl _userBl;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserBl userBl, ILogger<UserController> logger)
        {
            _userBl = userBl;
            _logger = logger;
        }
        [HttpPost]
        public IActionResult SighIn(UserDTO user)
        {
            try
            {
                _userBl.SighIn(user);
                BaseResponse<User> baseResponse = _userBl.SighIn(user);
                if (baseResponse.IsSucsses)
                    return StatusCode(baseResponse.StatusCode, baseResponse.Data);
                return StatusCode(baseResponse.StatusCode, baseResponse.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error on SighIn, Message: {ex.Message}," +
                                        $" InnerException: {ex.InnerException}, StackTrace: {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        public IActionResult LogIn(string password, string email)
        {
            try
            {
                BaseResponse<User> baseResponse = _userBl.Login(password, email);
                if (baseResponse.IsSucsses)
                    return StatusCode(baseResponse.StatusCode, baseResponse.Data);
                return StatusCode(baseResponse.StatusCode, baseResponse.Message);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error on Login, Message: {ex.Message}," +
                        $" InnerException: {ex.InnerException}, StackTrace: {ex.StackTrace}");

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }
        [HttpGet]
        public IActionResult LogOut()
        {
            try
            {
                _userBl.LogOut();
                BaseResponse<User> baseResponse = _userBl.LogOut();
                if (baseResponse.IsSucsses)
                    return StatusCode(baseResponse.StatusCode, baseResponse.Data);
                return StatusCode(baseResponse.StatusCode, baseResponse.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error on LogOut, Message: {ex.Message}," +
                        $" InnerException: {ex.InnerException}, StackTrace: {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }
    }
}
