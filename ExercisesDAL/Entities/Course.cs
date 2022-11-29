using System;
using System.Collections.Generic;

namespace ExercisesDAL.Entities
{
    public partial class Course : SchoolEntity
    {
        public Course()
        {
            Grades = new HashSet<Grade>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public int Credits { get; set; }
        public int DivisionId { get; set; }
        public byte[] Timer { get; set; } = null!;

        public virtual Division Division { get; set; } = null!;
        public virtual ICollection<Grade> Grades { get; set; }
    }
}
