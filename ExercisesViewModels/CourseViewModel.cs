using ExercisesDAL.DAOs;
using ExercisesDAL.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExercisesViewModels
{
    public class CourseViewModel
    {
        readonly private CourseDAO _dao;

        public int Id { get; set; }
        public string? Name { get; set; }
        public int Credits { get; set; }
        public int DivisionId { get; set; }


        public CourseViewModel()
        {
            _dao = new CourseDAO();
        }

        public async Task<List<CourseViewModel>> GetAllByStudentID(int studentID)
        {
            List<CourseViewModel> allvms = new();
            try
            {
                List<Course> allCourses = await _dao.GetAllByStudent(studentID);
                // we need to convert student instance to studentviewmodel because
                // the web layer isn't aware of the domain class student
                foreach (Course div in allCourses)
                {
                    CourseViewModel stuvm = new()
                    {
                        Id = div.Id,
                        Credits = div.Credits,

                        Name = div.Name,
                        DivisionId = div.DivisionId,
                        //Credits = div.Credits
                    };

                    allvms.Add(stuvm);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return allvms;
        }


    }
}
