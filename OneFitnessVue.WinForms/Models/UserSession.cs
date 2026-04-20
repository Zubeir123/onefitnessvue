namespace FitnessTimeGym.WinForms.Models;

public class UserSession
{
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string EmailId { get; set; } = string.Empty;
    public string MobileNo { get; set; } = string.Empty;
    public bool Status { get; set; }
}
