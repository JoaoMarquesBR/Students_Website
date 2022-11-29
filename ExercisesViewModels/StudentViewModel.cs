using ExercisesDAL.DAOs;
using ExercisesDAL.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
namespace ExercisesViewModels
{

    public class StudentViewModel
    {
        readonly private StudentDAO _dao;
        public string? Title { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }
        public string? Phoneno { get; set; }
        public string? Timer { get; set; }
        public int DivisionId { get; set; } // scaffolding left this non-nullable
        public string? DivisionName { get; set; }
        public int? Id { get; set; }
        public string? Picture64 { get; set; }
        // constructor
        public StudentViewModel()
        {
            _dao = new StudentDAO();
        }
        //
        // find employee using email property
        //
        public async Task GetByLastname()
        {
            try
            {
                Student stu = await _dao.GetByLastname(Lastname!);
                Title = stu.Title;
                Firstname = stu.FirstName;
                Lastname = stu.LastName;
                Phoneno = stu.PhoneNo;
                Email = stu.Email;
                Id = stu.Id;
                DivisionId = stu.DivisionId;

                if (stu.Picture != null)
                {
                    Picture64 = Convert.ToBase64String(stu.Picture);
                }
                Timer = Convert.ToBase64String(stu.Timer!);
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                Lastname = "not found";
            }
            catch (Exception ex)
            {
                Lastname = "not found";
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                 MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }



        public async Task getById()
        {
            try
            {
                Student stu = await _dao.GetId((int)Id!);
                Title = stu.Title;
                Firstname = stu.FirstName;
                Lastname = stu.LastName;
                Phoneno = stu.PhoneNo;
                Email = stu.Email;
                Id = stu.Id;
                DivisionId = stu.DivisionId;
                if (stu.Picture != null)
                {
                    Picture64 = Convert.ToBase64String(stu.Picture);
                }
                Timer = Convert.ToBase64String(stu.Timer!);
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                Lastname = "not found";
            }
            catch (Exception ex)
            {
                Lastname = "not found";
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                 MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }

        //
        // Retrieve all the students as ViewModel instances
        //
        public async Task<List<StudentViewModel>> GetAll()
        {
            List<StudentViewModel> allVms = new();
            try
            {
                List<Student> allStudents = await _dao.GetAll();
                // we need to convert Student instance to StudentViewModel because
                // the Web Layer isn't aware of the Domain class Student
                foreach (Student stu in allStudents)
                {
                    StudentViewModel stuVm = new()
                    {
                        Title = stu.Title,
                        Firstname = stu.FirstName,
                        Lastname = stu.LastName,
                        Phoneno = stu.PhoneNo,
                        Email = stu.Email,
                        Id = stu.Id,
                        DivisionId = stu.DivisionId,
                        DivisionName = stu.Division.Name,
                        // binary value needs to be stored on client as base64
                        Timer = Convert.ToBase64String(stu.Timer!)
                    };
                    if (stu.Picture != null)
                    {
                        stuVm.Picture64 = Convert.ToBase64String(stu.Picture);
                    }
                    allVms.Add(stuVm);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return allVms;
        }


        public async Task Add()
        {
            Id = -1;
            try
            {
                Student stu = new()
                {
                    Title = Title,   //trigger
                    FirstName = Firstname,
                    LastName = Lastname,
                    Email = Email, //did not trigger
                    PhoneNo = Phoneno, //did not drigger
                    DivisionId = DivisionId, //triggered

                    //Title = "Mr.",
                    //FirstName = "Joao",
                    //LastName = "Student",
                    //Email = "student@abc.com",
                    //PhoneNo = "(555)555-5551",
                    //DivisionId = 10 // ensu

                };

                if (Picture64 != null)
                {
                    stu.Picture = Convert.FromBase64String(Picture64);
                }

                Id = await _dao.Add(stu);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }

        public async Task<int> Update()
        {
            int updatestatus;
            try
            {
                Student stu = new()
                {
                    Title = Title,
                    FirstName = Firstname,
                    LastName = Lastname,
                    PhoneNo = Phoneno,
                    Email = Email,
                    Id = (int)Id!,
                    DivisionId = DivisionId,

                };

                if (Picture64 != null)
                {
                    stu.Picture = Convert.FromBase64String(Picture64);
                }

                stu.Timer = Convert.FromBase64String(Timer!);

                // dao will pass UpdateStatus of 1, -1, or -2
                updatestatus = -1;
                updatestatus = (int)await _dao.Update(stu);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }


            return updatestatus;
        }


        public async Task<int> Delete()
        {
            try
            {
                // dao will return # of rows deleted
                return await _dao.Delete((int)Id!);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }


    }
}