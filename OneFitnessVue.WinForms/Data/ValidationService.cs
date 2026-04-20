using System.Text.RegularExpressions;
using FitnessTimeGym.WinForms.Models;

namespace FitnessTimeGym.WinForms.Data;

public class ValidationService
{
    private static readonly Regex NameRegex = new("^[a-zA-Z ]*$", RegexOptions.Compiled);
    private static readonly Regex MobileRegex = new("^[0-9+]{9,15}$", RegexOptions.Compiled);
    private static readonly Regex EmailRegex = new(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.Compiled);

    public string ValidateMember(MemberFormModel model)
    {
        if (string.IsNullOrWhiteSpace(model.FirstName) || !NameRegex.IsMatch(model.FirstName))
        {
            return "Enter valid first name.";
        }

        if (string.IsNullOrWhiteSpace(model.LastName) || !NameRegex.IsMatch(model.LastName))
        {
            return "Enter valid last name.";
        }

        if (!string.IsNullOrWhiteSpace(model.MiddleName) && !NameRegex.IsMatch(model.MiddleName))
        {
            return "Enter valid middle name.";
        }

        if (string.IsNullOrWhiteSpace(model.MobileNo) || !MobileRegex.IsMatch(model.MobileNo))
        {
            return "Enter valid mobile number.";
        }

        if (!string.IsNullOrWhiteSpace(model.EmailId) && !EmailRegex.IsMatch(model.EmailId))
        {
            return "Enter valid email.";
        }

        if (model.Age <= 0)
        {
            return "Age must be greater than zero.";
        }

        if (model.MembershipTypeId <= 0 || model.WorkoutId <= 0 || model.InstallmentId <= 0)
        {
            return "Membership type, workout, and installment are required.";
        }

        if (model.PaymentTypeId <= 0)
        {
            return "Payment type is required.";
        }

        if (string.IsNullOrWhiteSpace(model.Address))
        {
            return "Address is required.";
        }

        if (string.IsNullOrWhiteSpace(model.EmergencyContactName) || !NameRegex.IsMatch(model.EmergencyContactName))
        {
            return "Enter valid emergency contact name.";
        }

        if (string.IsNullOrWhiteSpace(model.EmergencyContactNo))
        {
            return "Emergency contact number is required.";
        }

        return string.Empty;
    }
}
