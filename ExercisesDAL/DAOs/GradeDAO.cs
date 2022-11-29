using ExercisesDAL.Entities;
using ExercisesDAL.Repository;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection;


namespace ExercisesDAL.DAOs
{
    public class GradeDAO
    {
        readonly IRepository<Grade> _repo;
        public GradeDAO()
        {
            _repo = new SomeSchoolRepository<Grade>();
        }

      

        public async Task<List<Grade>> GetALlGradesByStudentID(int studentID)
        {
            return await _repo.GetSome(grade => grade.StudentId == studentID);
        }


        public async Task<int> Add(Grade newGrade)
        {
            return (await _repo.Add(newGrade)).Id;
        }
        public async Task<UpdateStatus> Update(Grade updatedGrade)
        {
            //return await _repo.Update(updatedGrade);    
            UpdateStatus status = UpdateStatus.Failed;
            try
            {
                SomeSchoolContext _db = new();
                Grade? currentStudent = await _db.Grades.FirstOrDefaultAsync(stu => stu.Id == updatedGrade.Id);
                _db.Entry(currentStudent!).OriginalValues["Timer"] = updatedGrade.Timer;
                _db.Entry(currentStudent!).CurrentValues.SetValues(updatedGrade);
                if (await _db.SaveChangesAsync() == 1)
                {
                    status = UpdateStatus.Ok;
                };
            }
            catch (DbUpdateConcurrencyException)
            {
                status = UpdateStatus.Stale;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }

            return status;

        }


        public async Task<int> Delete(int id)
        {
            int gradeSelected = -1;
            try
            {
                gradeSelected = await _repo.Delete(id!);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return gradeSelected;
        }

   
    }
}