﻿using System;
using System.Collections.Generic;

namespace ExercisesDAL.Entities
{
    public partial class Call
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int ProblemId { get; set; }
        public int TechId { get; set; }
        public DateTime DateOpened { get; set; }
        public DateTime? DateClosed { get; set; }
        public bool OpenStatus { get; set; }
        public string Notes { get; set; } = null!;
        public byte[] Timer { get; set; } = null!;

        public virtual Employee Employee { get; set; } = null!;
        public virtual Problem Problem { get; set; } = null!;
        public virtual Employee Tech { get; set; } = null!;
    }
}
