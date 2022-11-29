using System;
using System.Collections.Generic;

namespace ExercisesDAL.Entities
{
    public partial class Problem
    {
        public Problem()
        {
            Calls = new HashSet<Call>();
        }

        public int Id { get; set; }
        public string? Description { get; set; }
        public byte[] Timer { get; set; } = null!;

        public virtual ICollection<Call> Calls { get; set; }
    }
}
