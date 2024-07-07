using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QueueManagmentApi.Attributes;
using QueueManagmentBL.Interfaces;
using QueueManagmentDB.EF.Models;
using QueueManagmentEntites;

namespace QueueManagmentApi.Controllers
{
    //-token
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class QueController : Controller
    {
        private readonly ILogger<QueController> _logger;
        private readonly IQueBl _queBl;
        public QueController(IQueBl queBl, ILogger<QueController> logger)
        {
            _queBl = queBl;
            _logger = logger;
        }

        [HttpPost]
        [ServiceFilter(typeof(ConflictAttribute))]
        public IActionResult AddNewQue(QueueDTO queue)
        {
            try
            {
                BaseResponse<Queue> baseResponse = _queBl.AddNewQue(queue);
                if (baseResponse.IsSucsses)
                    return StatusCode(baseResponse.StatusCode, baseResponse.Data);
                return StatusCode(baseResponse.StatusCode, baseResponse.Message);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error on AddNewQue, Message: {ex.Message}," +
                        $" InnerException: {ex.InnerException}, StackTrace: {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPut("/{id}")]
        [ServiceFilter(typeof(ConflictAttribute))]
        public IActionResult EditQueue(QueueDTO queue, [FromRoute] int id)
        {
            try
            {

                BaseResponse<Queue> baseResponse = _queBl.EditQueue(id, queue);
                if (baseResponse.IsSucsses)
                    return StatusCode(baseResponse.StatusCode, baseResponse.Data);
                return StatusCode(baseResponse.StatusCode, baseResponse.Message);


            }
            catch (Exception ex)
            {
                _logger.LogError($"Error on EditQueue, Message: {ex.Message}," +
                        $" InnerException: {ex.InnerException}, StackTrace: {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpDelete("/{id}")]
        public IActionResult RemoveQue([FromRoute] int id)
        {
            try
            {
                BaseResponse<Queue> baseResponse = _queBl.RemoveQue(id);
                if (baseResponse.IsSucsses)
                    return StatusCode(baseResponse.StatusCode, baseResponse.Data);
                return StatusCode(baseResponse.StatusCode, baseResponse.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error on RemoveQue, Message: {ex.Message}," +
                        $" InnerException: {ex.InnerException}, StackTrace: {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAllQues()
        {
            try
            {
                BaseResponse<List<Queue>> baseResponse = _queBl.GetAllQues();
                if (baseResponse.IsSucsses)
                    return StatusCode(baseResponse.StatusCode, baseResponse.Data);
                return StatusCode(baseResponse.StatusCode, baseResponse.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error on GetAllQues, Message: {ex.Message}," +
                        $" InnerException: {ex.InnerException}, StackTrace: {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }

    }
}
