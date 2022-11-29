using ExercisesDAL.Entities;
using ExercisesDAL.Repository;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection;
namespace ExercisesDAL.DAOs
{
    public class StudentDAO
    {
        readonly IRepository<Student> _repo;
        public StudentDAO()
        {
            _repo = new SomeSchoolRepository<Student>();
        }

        public async Task<Student> GetByLastname(string? name)
        {
            Student? selectedStudent;
            try
            {

                selectedStudent = await _repo.GetOne(stu => stu.LastName == name);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return selectedStudent!;
        }



        public async Task<Student> GetId(int id)
        {
            Student? selectedStudent;
            try
            {
                selectedStudent = await _repo.GetOne(stu => stu.Id == id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return selectedStudent!;
        }

        public async Task<List<Student>> GetAll()
        {
            List<Student> allStudents;
            try
            {
                //SomeSchoolContext _db = new();
                //allStudents = await _db.Students.ToListAsync();
                allStudents = await _repo.GetAll();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return allStudents;
        }


        public async Task<int> Add(Student newStudent)
        {
            try
            {
                newStudent = await _repo.Add(newStudent);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return newStudent.Id;
        }


        //public async Task<UpdateStatus> Update(Student updatedStudent)
        //{
        //    UpdateStatus status;

        //    try
        //    {
        //        status = await _repo.Update(updatedStudent);
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine("Problem in " + GetType().Name + " " +
        //        MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
        //        throw;
        //    }
        //    return status;
        //}
        public async Task<UpdateStatus> Update(Student updatedStudent)
        {
            UpdateStatus status = UpdateStatus.Failed;
            try
            {
                SomeSchoolContext _db = new();
                Student? currentStudent = await _db.Students.FirstOrDefaultAsync(stu => stu.Id == updatedStudent.Id);
                _db.Entry(currentStudent!).OriginalValues["Timer"] = updatedStudent.Timer;
                _db.Entry(currentStudent!).CurrentValues.SetValues(updatedStudent);
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
            int studentsDeleted = -1;
            try
            {
                studentsDeleted = await _repo.Delete(id!);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return studentsDeleted;
        }

    }
}