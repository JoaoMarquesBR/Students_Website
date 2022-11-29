using Castle.Components.DictionaryAdapter;
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
    public class GradeViewModel
    {
        readonly private GradeDAO _dao;
        readonly private CourseDAO _courseDAO;

        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public int Mark { get; set; }
        public string? Comments { get; set; }

        public string? Timer { get; set; }
        public int Id { get; set; }

        public GradeViewModel()
        {
            _dao = new GradeDAO();
        }

        public async Task<List<GradeViewModel>> GetGradesByCourse(int studentID)
        {
            List<GradeViewModel> allvms = new();
            try
            {
                List<Grade> allCourses = await _dao.GetALlGradesByStudentID(studentID);
                // we need to convert student instance to studentviewmodel because
                // the web layer isn't aware of the domain class student
                foreach (Grade div in allCourses)
                {
                    GradeViewModel newGrade = new()
                    {
                        Id = div.Id,
                        StudentId = studentID,  
                        CourseId = div.CourseId,
                        Mark = div.Mark,
                        Comments = div.Comments,
                        Timer = Convert.ToBase64String(div.Timer!)
                        
                        //Credits = div.Credits
                    };

                    allvms.Add(newGrade);
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


        public async Task<int> Update()
        {
            int updatestatus;
            try
            {
                Grade newGrade = new()
                {
                    Id = Id,
                    Mark = Mark,
                    Comments = Comments,
                    CourseId = CourseId,
                    StudentId = StudentId,
                    Timer = Convert.FromBase64String(this.Timer)
                };


                updatestatus = -1;
                if (newGrade != null)
                {
                    updatestatus = (int)await _dao.Update(newGrade);
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }


            return updatestatus;
        }
    }
}
