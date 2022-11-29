using ExercisesDAL;
using ExercisesDAL.Entities;
using Microsoft.EntityFrameworkCore;



namespace ExercisesTests
{

    public class LinqTests
    {
        [Fact]
        public void Test1()
        {
            SomeSchoolContext _db = new();

            var selectedStudents = from stu in _db.Students     //select statement, opposite! very old school
                                   where stu.Id == 2            //no ones does that way anymore
                                   select stu;

            Assert.True(selectedStudents.Any());     //check if there's data inside
        }

        [Fact]
        public void Test2()
        {
            SomeSchoolContext _db = new();

            var selectedStudents = from stu in _db.Students
                                   where stu.Title == "Ms." || stu.Title == "Mrs."

                                   select stu;

            Assert.True(selectedStudents.Any());
        }


        [Fact]
        public void Test3()
        {
            SomeSchoolContext _db = new();

            var selectedStudents = from stu in _db.Students     //div 50 = design
                                   where stu.DivisionId == 50
                                   select stu;



            Assert.True(selectedStudents.Any());     //check if there's data inside
        }


        [Fact]
        public void Test4()
        {
            SomeSchoolContext _db = new();
            Student? selectedStudents = _db.Students.FirstOrDefault(stu => stu.Id == 2); //second part is like a if statement
            //always use firstOrDefault, bc it returns safe null if doesnt find anything
            //for the only FIRST, it would throw an error

            Assert.True(selectedStudents!.FirstName == "Gail");
        }


        [Fact]
        public void Test5()
        {
            SomeSchoolContext _db = new();
            var selectedStudents = _db.Students.Where(stu => stu.Title == "Ms." || stu.Title == "Mrs.");
            //use FirstOrDefault for just ONE row being retrieved
            //use WHERE for multiple rows

            Assert.True(selectedStudents.Any());     //check if there's data inside.
        }

        [Fact]
        public void Test6()
        {
            SomeSchoolContext _db = new();
            var selectedStudents = _db.Students.Where(stu => stu.Division.Name == "Design");

            Assert.True(selectedStudents.Any());     //check if there's data inside.
        }




        [Fact]
        public async void A_Test8()
        {
            SomeSchoolContext _db = new();
            Student mynewstudent = new();
            mynewstudent.FirstName = "joao";
            mynewstudent.LastName = "Santos";
            mynewstudent.PhoneNo = "(519)-615-4641";
            mynewstudent.Title = "mr.";
            mynewstudent.DivisionId = 50;
            mynewstudent.Email = "joao@fansa.ca";
            await _db.Students.AddAsync(mynewstudent);
            await _db.SaveChangesAsync();
            Assert.True(mynewstudent.Id > 0);     //check if there's data inside.
        }

        [Fact]
        public async void B_Test7() //updating value of database
        {
            SomeSchoolContext _db = new();
            Student? selectedstudents = await _db.Students.FirstOrDefaultAsync(stu => stu.LastName == "Santos");
            if (selectedstudents != null)
            {
                string oldEmail = selectedstudents.Email;
                string newEmail = oldEmail == "joao@fansa.ca" ? "j_marquesdossantos@fanshaweonline.ca" : "joao@fansa.ca";
                selectedstudents.Email = newEmail;
                _db.Entry(selectedstudents).CurrentValues.SetValues(selectedstudents);
            }
            Assert.True(await _db.SaveChangesAsync() == 1);     //save the changes on the database
        }


        [Fact]
        public async void test9()
        {
            SomeSchoolContext _db = new();
            Student? selectedstudent = await _db.Students.FirstOrDefaultAsync(stu => stu.LastName == "Santos");

            if (selectedstudent != null)
            {
                _db.Students.Remove(selectedstudent);
                Assert.True(await _db.SaveChangesAsync() == 1);
            }
            else
            {
                Assert.True(false);
            }
        }


    }


}