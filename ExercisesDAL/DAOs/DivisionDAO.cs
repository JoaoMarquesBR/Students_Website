using ExercisesDAL.Entities;
using ExercisesDAL.Repository;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection;
namespace ExercisesDAL.DAOs
{
    public class DivisionDAO
    {
        readonly IRepository<Division> _repo;
        public DivisionDAO()
        {
            _repo = new SomeSchoolRepository<Division>();
        }

        public async Task<List<Division>> GetAll()
        {
            List<Division> allDivisions;
            try
            {
                //SomeSchoolContext _db = new();
                //allStudents = await _db.Students.ToListAsync();
                allDivisions = await _repo.GetAll();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return allDivisions;
        }


    }
}