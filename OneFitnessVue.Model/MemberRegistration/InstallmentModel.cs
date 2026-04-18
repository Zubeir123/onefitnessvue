using System.ComponentModel.DataAnnotations;

namespace FitnessTimeGym.Model.Installments
{
    public class InstallmentModel
    {
        [Key]
        public int InstallmentId { get; set; }

        public string InstallmentName { get; set; }

        public int InstallmentMonths { get; set; }

        public bool Status { get; set; }
    }
}