using ExercisesDAL.Entities;
using ExercisesViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection;

namespace ExercisesWebsite.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class GradeController : Controller
    {

        
        [HttpGet("{studentID}")]
        public async Task<IActionResult> GetByCourseAndStudent(int studentID)
        {
            try
            {
                GradeViewModel viewmodel = new();
                List<GradeViewModel> allGrades = await viewmodel.GetGradesByCourse(studentID);
                return Ok(allGrades);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }

        [HttpPut]
        public async Task<ActionResult> Put(GradeViewModel viewmodel)
        {
            try
            {
                int retVal = await viewmodel.Update();
                return retVal switch
                {
                    1 => Ok(new { msg = "Student " + viewmodel.CourseId + " updated!" }),
                    -1 => Ok(new { msg = "Student " + viewmodel.CourseId + " not updated!" }),
                    -2 => Ok(new { msg = "Data is stale for " + viewmodel.CourseId + ", Student not updated!" }),
                    -3 => Ok(new { msg = "Student " + viewmodel.CourseId + " not updated!" }),
                };
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
