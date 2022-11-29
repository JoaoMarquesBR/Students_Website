using Xunit;
using ExercisesDAL;
using System.Threading.Tasks;
using System.Collections.Generic;
using ExercisesDAL.Entities;
using ExercisesDAL.DAOs;

namespace ExercisesTests
{
    public class DAOTests
    {
        [Fact]
        public async Task Student_GetByLastnameTest()
        {
            StudentDAO dao = new();
            Student selectedStudent = await dao.GetByLastname("Cross");
            Assert.NotNull(selectedStudent);
        }

        [Fact]
        public async Task Student_GetById() 
        {
            StudentDAO dao = new();
            Student selectedStudent = await dao.GetId(1);
            Assert.NotNull(selectedStudent);
        }

        [Fact]
        public async Task Student_GetAll() { 
            StudentDAO dao = new();
            List<Student> allStudents = await dao.GetAll();
            Assert.True(allStudents.Count > 0);
        }

         [Fact]
        public async void Student_Add()
        {
            StudentDAO dao = new();
            Student mynewstudent = new()
            {
                Title = "Mr.",
                FirstName = "Joe",
                LastName = "Smith",
                Email = "some@abc.com",
                PhoneNo = "(555)555-5551",
                DivisionId = 10 // ensure division id is in Division table
            };
            Assert.True(await dao.Add(mynewstudent) > 0);     //check if there's data inside.
        }

        [Fact]
        public async Task Student_UpdateTest()
        {
            StudentDAO dao = new();
            Student? studentForUpdate = await dao.GetByLastname("Smith");
            if (studentForUpdate != null)
            {
                string oldPhoneNo = studentForUpdate.PhoneNo!;
                string newPhoneNo = oldPhoneNo == "519-555-1234" ? "555-555-5555" : "519-555-1234";
                studentForUpdate!.PhoneNo = newPhoneNo;
            }
            Assert.True(await dao.Update(studentForUpdate!) == UpdateStatus.Ok); // 1 indicates the # of rows updated
        }
        [Fact]
        public async Task Student_ConcurrencyTest()
        {
            StudentDAO dao1 = new();
            StudentDAO dao2 = new();
            Student studentForUpdate1 = await dao1.GetByLastname("Smith");
            Student studentForUpdate2 = await dao2.GetByLastname("Smith");
            if (studentForUpdate1 != null)
            {
                string? oldPhoneNo = studentForUpdate1.PhoneNo;
                string? newPhoneNo = oldPhoneNo == "519-555-1234" ? "555-555-5555" : "519-555-1234";
                studentForUpdate1.PhoneNo = newPhoneNo;
                if (await dao1.Update(studentForUpdate1) == UpdateStatus.Ok)
                {
                    // need to change the phone # to something else
                    studentForUpdate2.PhoneNo = "666-666-6668";
                    Assert.True(await dao2.Update(studentForUpdate2) == UpdateStatus.Stale);
                }
                else
                    Assert.True(false); // first update failed
            }
            else
                Assert.True(false); // didn't find student 1
        }


        [Fact]
        public async Task Student_Delete()
        {
            StudentDAO dao = new();
            Student? studentForDelete = await dao.GetByLastname("Smith");
            Assert.True(await dao.Delete(studentForDelete.Id) == 1); // 1 indicates the # of rows updated
        }



       

    }
}