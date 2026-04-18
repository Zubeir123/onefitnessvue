using System;
using System.ComponentModel.DataAnnotations;

namespace FitnessTimeGym.Model.MembershipType
{
    public class MembershipTypeModel
    {
        [Key]
        public int MembershipTypeId { get; set; }

        public string MembershipTypeName { get; set; }

        public decimal Amount { get; set; }

        public int InstallmentId { get; set; }

        public int WorkOutId { get; set; }

        public bool Status { get; set; }

        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int ModifiedBy { get; set; }
    }
}

