using System;
using System.Collections.Generic;

namespace ExercisesDAL.Entities
{
    public partial class Division : SchoolEntity
    {
        public Division()
        {
            Courses = new HashSet<Course>();
            Students = new HashSet<Student>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public byte[] Timer { get; set; } = null!;

        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}
