using ExercisesViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection;

namespace ExercisesWebsite.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DivisionController : Controller
    {

        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                DivisionViewModel viewmodel = new();
                List<DivisionViewModel> allStudents = await viewmodel.getall();
                return Ok(allStudents);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }
    }
}
