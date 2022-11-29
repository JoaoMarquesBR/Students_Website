using ExercisesViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection;

namespace ExercisesWebsite.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class Test : Controller
    {

        
        [HttpGet("{studentID}")]
        public async Task<IActionResult> GetAll(int studentID)
        {
            try
            {
                CourseViewModel viewmodel = new();
                List<CourseViewModel> allCourses = await viewmodel.GetAllByStudentID(studentID);
                return Ok(allCourses);
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
