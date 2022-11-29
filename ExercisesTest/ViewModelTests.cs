using Xunit;
using ExercisesViewModels;
using System.Threading.Tasks;
using System.Runtime.Intrinsics.X86;

namespace ExercisesTests
{
    public class ViewModelTests
    {
        [Fact]
        public async Task Student_GetByLastnameTest()
        {
            StudentViewModel vm = new() { Lastname = "Smith" };
            await vm.GetByLastname();
            Assert.NotNull(vm.Firstname);
        }

        [Fact]
        public async Task Student_GetByIdTest()
        {
            StudentViewModel vm = new() { Id = 1 };
            await vm.getById();
            Assert.NotNull(vm.Id);
        }

        [Fact]
        public async Task Student_GetAllTest()
        {
            List<StudentViewModel> allStudentsVms;
            StudentViewModel vm = new();
            allStudentsVms = await vm.GetAll();
            Assert.True(allStudentsVms.Count > 0);
        }


        [Fact]
        public async Task Student_AddTest()
        {
            StudentViewModel vm;
            vm = new()
            {
                Title = "Mr.",
                Firstname = "Joao",
                Lastname = "Student",
                Email = "student@abc.com",
                Phoneno = "(555)555-5551",
                DivisionId = 10 // ensure division id is in Division table
            };
            await vm.Add();
            Assert.True(vm.Id > 0);
        }

        [Fact]
        public async Task Student_UpdateTest()
        {
            StudentViewModel vm = new() { Lastname = "Smith" };
            await vm.GetByLastname(); // Student just added in Add test
            vm.Phoneno = vm.Phoneno == "(555)555-5551" ? "(555)555-5552" : "(555)555-5551";
            // will be -1 if failed 0 if no data changed, 1 if succcessful
            Assert.True(await vm.Update() == 1);
            // vm.
        }


        [Fact]
        public async Task Student_DeleteTest()
        {
            StudentViewModel vm = new() { Lastname = "Student" };
            await vm.GetByLastname(); // Student just added
            Assert.True(await vm.Delete() == 1); // 1 student deleted
        }

        [Fact]
        public async Task Student_ConcurrencyTest()
        {
            StudentViewModel vm1 = new() { Lastname = "Student" };
            StudentViewModel vm2 = new() { Lastname = "Student" };
            await vm1.GetByLastname(); // Fetch same student to simulate 2 users
            if (vm1.Lastname != "Not Found") // make sure we found a student
            {
                await vm2.GetByLastname(); // fetch same data
                vm1.Phoneno = vm1.Phoneno == "(555)555-5551" ? "(555)555-5552" : "(555)555-5551";
                if (await vm1.Update() == 1)
                {
                    vm2.Phoneno = "(666)666-6666"; // just need any value
                    Assert.True(await vm2.Update() == -2);
                }
            }
            else
            {
                Assert.True(false); // student not found
            }
        }


    }

}
