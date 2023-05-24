using Microsoft.AspNetCore.Mvc;
using RESTcontrollers.Models;
using RESTcontrollers.Services;

namespace RESTcontrollers.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/examination")]
    [ApiController]
    public class ExaminationController : Controller
    {
        //Handle other requests! Don't forget to Change the classes!
        private readonly ExaminationService service;

        public ExaminationController()
        {
            service = new ExaminationService();
        }

        [HttpPost("/Add")]
        public IActionResult UploadExamination()
        {
            // [FromBody]Examination examination
            Examination examination = new Examination();
            ExaminationService examinationService = new ExaminationService();
            bool result = examinationService.SendRequest(HttpMethod.Post, "http://localhost:7103/api/Function1", examination);
            if (!result)
            {
                return Ok("Table has element like this");
            }
            return Ok(result);
        }

        public IActionResult GetExaminations()
        {
            return Ok(service.GetRequest(HttpMethod.Get, "",""));
        }
    }
}
