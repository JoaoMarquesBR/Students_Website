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
    public class DivisionViewModel
    {
        readonly private DivisionDAO _dao;

        public string? timer { get; set; }
        public string? name { get; set; }
        public int? id { get; set; }
        public DivisionViewModel()
        {
            _dao = new DivisionDAO();
        }

        public async Task<List<DivisionViewModel>> getall()
        {
            List<DivisionViewModel> allvms = new();
            try
            {
                List<Division> allstudents = await _dao.GetAll();
                // we need to convert student instance to studentviewmodel because
                // the web layer isn't aware of the domain class student
                foreach (Division stu in allstudents)
                {
                    DivisionViewModel stuvm = new()
                    {
                        id = stu.Id,
                        name = stu.Name,
                        // binary value needs to be stored on client as base64
                        timer = Convert.ToBase64String(stu.Timer!)
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
