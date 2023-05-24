using CommandLineInterface;
using Microsoft.AspNetCore.Mvc;
using RESTcontrollers.Services;

namespace RESTcontrollers.Controllers
{
    //[Route("api/[controller]")]
    //Ezen attributumok szükségesek
    //Meg kell nézni a portot a felugró RestControllers ablakon!
    [Route("api/examination")]
    [ApiController]
    public class ExaminationController : Controller
    {
        //Handle other requests!
        private readonly ExaminationService service;

        public ExaminationController()
        {
            service = new ExaminationService();
        }
        //https://localhost:7252/Add
        [HttpPost("/Add")]
        public IActionResult UploadExamination()
        {
            // [FromBody]Examination examination
            Examination examination = new Examination();
            //Service példány, hogy meghívjuk a metódusait:
            ExaminationService examinationService = new ExaminationService();
            //Hívás elküldése, eredményre várakozás: Service kommentek.
            bool result = examinationService.SendRequest(HttpMethod.Post, "http://localhost:7103/api/Function1", examination);
            if (!result)
            {
                return Ok("Table has element like this");
            }
            return Ok(result);
        }

        //Olvasást itt majd meg kell oldani
        public IActionResult GetExaminations()
        {
            return Ok(service.GetRequest(HttpMethod.Get, "",""));
        }
    }
}
