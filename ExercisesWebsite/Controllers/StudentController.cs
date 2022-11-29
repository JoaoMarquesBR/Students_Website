using ExercisesViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection;
namespace ExercisesWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {

        //client is requesting data.
        [HttpGet("{lastname}")]
        public async Task<IActionResult> GetByLastname(string lastname) //wait to get on httpget

        {
            try
            {
                StudentViewModel viewmodel = new() { Lastname = lastname };
                await viewmodel.GetByLastname();
                return Ok(viewmodel);//200 means OK 
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); //500 means ERROR on the SERVER
                                                                             //database wasn't found in the lower layers.
            }
        }

        [HttpPut]
        public async Task<ActionResult> Put(StudentViewModel viewmodel)
        {
            try
            {
                int retVal = await viewmodel.Update();
                return retVal switch
                {
                    1 => Ok(new { msg = "Student " + viewmodel.Lastname + " updated!" }),
                    -1 => Ok(new { msg = "Student " + viewmodel.Lastname + " not updated!" }),
                    -2 => Ok(new { msg = "Data is stale for " + viewmodel.Lastname + ", Student not updated!" }),
                    _ => Ok(new { msg = "Student " + viewmodel.Lastname + " not updated!" }),
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }



        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                StudentViewModel viewmodel = new();
                List<StudentViewModel> allStudents = await viewmodel.GetAll();
                return Ok(allStudents);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post(StudentViewModel viewmodel)
        {
            try
            {
                Console.WriteLine("Viel model as \n\n\n\n\n\n\n\n\n\n");
                await viewmodel.Add();
                return viewmodel.Id > 1
                ? Ok(new { msg = "Student " + viewmodel.Lastname + " added!" })
                : Ok(new { msg = "Student " + viewmodel.Lastname + " not added!" });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                StudentViewModel viewmodel = new() { Id = id };
                return await viewmodel.Delete() == 1
                ? Ok(new { msg = "Student " + id + " deleted!" })
               : Ok(new { msg = "Student " + id + " not deleted!" });
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
