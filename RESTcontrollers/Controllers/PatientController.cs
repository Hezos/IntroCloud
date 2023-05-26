using Microsoft.AspNetCore.Mvc;
using RESTcontrollers.Services;

namespace RESTcontrollers.Controllers
{
    [Route("api/patient")]
    [ApiController]
    public class PatientController : Controller
    {

        private readonly PatientService service;

        public PatientController()
        {
            service = new PatientService();
        }

        //https://localhost:7252/Add
        [HttpPost("/AddPatient")]
        public IActionResult UploadExamination([FromBody] Patient patient)
        {
            //Service példány, hogy meghívjuk a metódusait:
            PatientService examinationService = new PatientService();
            //Hívás elküldése, eredményre várakozás: Service kommentek.
            bool result = examinationService.SendRequest(HttpMethod.Post, "http://localhost:7103/api/Function1", patient);
            if (!result)
            {
                return Ok("Table has element like this");
            }
            return Ok(result);
        }


        [HttpGet("/GetPatients")]
        public IActionResult GetExaminations()
        {
            List<Examination> result = service.GetRequest(HttpMethod.Post, "http://localhost:7103/api/Function1", "");
            Console.WriteLine(result);

            return Ok(result);
        }
    }
}
