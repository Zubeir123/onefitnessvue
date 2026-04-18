using System;
using System.ComponentModel.DataAnnotations;

namespace FitnessTimeGym.Model.WorkOut
{
    public class WorkOutModel
    {
        [Key]
        public int Id { get; set; }
        public int WorkOutId { get; set; }

        public string WorkOutName { get; set; }

        public bool Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public int? ModifiedBy { get; set; }
    }
}