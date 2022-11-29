using System;
using System.Collections.Generic;

namespace ExercisesDAL.Entities
{
    public partial class Grade : SchoolEntity
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public int Mark { get; set; }
        public string? Comments { get; set; }
        public byte[] Timer { get; set; } = null!;

        public virtual Course Course { get; set; } = null!;
        public virtual Student Student { get; set; } = null!;
    }
}
