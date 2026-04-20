namespace FitnessTimeGym.WinForms.Models;

public class MemberFormModel
{
    public string MemberNo { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public DateTime DOB { get; set; }
    public int Age { get; set; }
    public string MobileNo { get; set; } = string.Empty;
    public string EmailId { get; set; } = string.Empty;
    public int GenderId { get; set; }
    public string Address { get; set; } = string.Empty;
    public DateTime JoiningDate { get; set; }
    public bool Status { get; set; } = true;
    public string EmergencyContactName { get; set; } = string.Empty;
    public string EmergencyContactNo { get; set; } = string.Empty;
    public int MembershipTypeId { get; set; }
    public int WorkoutId { get; set; }
    public int InstallmentId { get; set; }
    public int PaymentTypeId { get; set; }
    public int TaxId { get; set; }
}
