using ExercisesDAL.Entities;
using ExercisesDAL.Repository;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection;
namespace ExercisesDAL.DAOs
{
    public class CourseDAO
    {
        readonly IRepository<Course> _repo;
        GradeDAO gradeDAO = new GradeDAO();
        public CourseDAO()
        {
            _repo = new SomeSchoolRepository<Course>();
        }

        public async Task<List<Course>> GetAllByStudent(int studentID)
        {
            var gradeList = await gradeDAO.GetALlGradesByStudentID(studentID);

            List<Course> courseList = new List<Course>();
            foreach(Grade g in gradeList)
            {
                Course course = g.Course;
                courseList.Add(course);
            }

            return courseList;

        }


    }
}