using System;
using System.Collections.Generic;

namespace ExercisesDAL.Entities
{
    public partial class Department
    {
        public Department()
        {
            Employees = new HashSet<Employee>();
        }

        public int Id { get; set; }
        public string? DepartmentName { get; set; }
        public byte[] Timer { get; set; } = null!;

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
