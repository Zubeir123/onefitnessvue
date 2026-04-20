using FitnessTimeGym.Model.MemberRegistration;
using FitnessTimeGym.Model.PaymentDetails;
using FitnessTimeGym.WinForms.Data;
using FitnessTimeGym.WinForms.Models;

namespace FitnessTimeGym.WinForms.Forms;

public class AddMemberForm : Form
{
    private readonly GymRepository _repository;
    private readonly int _userId;
    private readonly ValidationService _validationService = new();
    private readonly CalculationService _calculationService = new();

    private readonly TextBox _txtMemberNo = new() { ReadOnly = true };
    private readonly TextBox _txtFirstName = new();
    private readonly TextBox _txtMiddleName = new();
    private readonly TextBox _txtLastName = new();
    private readonly TextBox _txtMobile = new();
    private readonly TextBox _txtEmail = new();
    private readonly DateTimePicker _dtDob = new() { Format = DateTimePickerFormat.Short };
    private readonly TextBox _txtAge = new() { ReadOnly = true };
    private readonly ComboBox _cmbMembershipType = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly ComboBox _cmbWorkout = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly ComboBox _cmbInstallment = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly ComboBox _cmbTax = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly DateTimePicker _dtJoining = new() { Format = DateTimePickerFormat.Short };
    private readonly TextBox _txtAddress = new();
    private readonly TextBox _txtEmergencyContactName = new();
    private readonly TextBox _txtEmergencyContactNo = new();
    private readonly Label _lblAmount = new() { AutoSize = true };
    private readonly Label _lblTax = new() { AutoSize = true };
    private readonly Label _lblTotal = new() { AutoSize = true };
    private readonly Button _btnSave = new() { Text = "Save Member", Width = 150, Height = 38 };

    public AddMemberForm(GymRepository repository, int userId)
    {
        _repository = repository;
        _userId = userId;
        InitializeLayout();
        LoadData();
    }

    private void InitializeLayout()
    {
        Text = "Add Member";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(920, 680);

        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 4,
            Padding = new Padding(20)
        };

        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

        AddField(panel, "Member No", _txtMemberNo, 0);
        AddField(panel, "First Name", _txtFirstName, 1);
        AddField(panel, "Middle Name", _txtMiddleName, 2);
        AddField(panel, "Last Name", _txtLastName, 3);
        AddField(panel, "Mobile", _txtMobile, 4);
        AddField(panel, "Email", _txtEmail, 5);
        AddField(panel, "DOB", _dtDob, 6);
        AddField(panel, "Age", _txtAge, 7);
        AddField(panel, "Membership Type", _cmbMembershipType, 8);
        AddField(panel, "Workout", _cmbWorkout, 9);
        AddField(panel, "Installment", _cmbInstallment, 10);
        AddField(panel, "Tax", _cmbTax, 11);
        AddField(panel, "Joining Date", _dtJoining, 12);
        AddField(panel, "Address", _txtAddress, 13);
        AddField(panel, "Emergency Name", _txtEmergencyContactName, 14);
        AddField(panel, "Emergency No", _txtEmergencyContactNo, 15);

        var amountPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            AutoSize = true
        };
        amountPanel.Controls.Add(_lblAmount);
        amountPanel.Controls.Add(_lblTax);
        amountPanel.Controls.Add(_lblTotal);
        panel.Controls.Add(amountPanel, 0, 16);
        panel.SetColumnSpan(amountPanel, 2);

        panel.Controls.Add(_btnSave, 2, 16);
        panel.SetColumnSpan(_btnSave, 2);

        _dtDob.ValueChanged += (_, _) => UpdateAge();
        _cmbMembershipType.SelectedIndexChanged += (_, _) => RefreshAmount();
        _cmbTax.SelectedIndexChanged += (_, _) => RefreshAmount();
        _btnSave.Click += OnSaveClick;

        Controls.Add(panel);
    }

    private void LoadData()
    {
        _txtMemberNo.Text = _repository.GenerateMemberNo();
        _dtDob.Value = DateTime.Today.AddYears(-18);
        _dtJoining.Value = DateTime.Today;
        _txtAddress.Text = "N/A";

        BindLookup(_cmbMembershipType, _repository.GetMembershipTypes());
        BindLookup(_cmbWorkout, _repository.GetWorkouts());
        BindLookup(_cmbInstallment, _repository.GetInstallments());
        BindLookup(_cmbTax, _repository.GetTaxTypes());

        UpdateAge();
        RefreshAmount();
    }

    private void OnSaveClick(object? sender, EventArgs e)
    {
        var model = CollectFormModel();
        var validationError = _validationService.ValidateMember(model);
        if (!string.IsNullOrWhiteSpace(validationError))
        {
            MessageBox.Show(validationError, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_repository.CheckMemberMobileNoExists(model.MobileNo))
        {
            MessageBox.Show("MobileNo Already Exists", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!string.IsNullOrWhiteSpace(model.EmailId) && _repository.CheckMemberEmailExists(model.EmailId))
        {
            MessageBox.Show("Email Already Exists", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var amount = _repository.GetMembershipAmount(model.MembershipTypeId);
        var tax = _repository.GetTaxPercentage(model.TaxId);
        var calculation = _calculationService.CalculateTotal(amount, tax);
        var installmentMonths = _repository.GetInstallmentMonths(model.InstallmentId);
        var nextRenewalDate = model.JoiningDate.AddMonths(installmentMonths);

        var member = new MemberRegistrationModel
        {
            MemberNo = model.MemberNo,
            FirstName = UpperFirst(model.FirstName),
            MiddleName = UpperFirst(model.MiddleName),
            LastName = UpperFirst(model.LastName),
            MobileNo = model.MobileNo,
            EmailId = model.EmailId,
            DOB = model.DOB,
            Age = model.Age,
            GenderId = model.GenderId,
            Address = model.Address,
            JoiningDate = model.JoiningDate,
            Status = true,
            EmergencyContactName = model.EmergencyContactName,
            EmergencyContactNo = model.EmergencyContactNo,
            CreatedBy = _userId,
            CreatedOn = DateTime.Now
        };

        var payment = new PaymentDetailsModel
        {
            MembershipTypeId = model.MembershipTypeId,
            WorkOutId = model.WorkoutId,
            PaymentTypeId = model.PaymentTypeId,
            PaymentFromdt = model.JoiningDate,
            PaymentTodt = nextRenewalDate,
            NextRenewalDate = nextRenewalDate,
            RecStatus = "A",
            TaxId = model.TaxId,
            TaxPercentage = calculation.TaxPercentage,
            Amount = calculation.Amount,
            TaxPercentageAmount = calculation.TaxAmount,
            TotalAmount = calculation.TotalAmount,
            InstallmentId = model.InstallmentId,
            MemberNo = model.MemberNo,
            ApplicationType = "NW",
            InvoiceNo = _repository.GetNextInvoiceNo(),
            CreatedBy = _userId,
            CreatedOn = DateTime.Now
        };

        var result = _repository.AddMemberWithPayment(member, payment);
        if (!result)
        {
            MessageBox.Show("Unable to add member.", "Save Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        MessageBox.Show("Member was added Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        ResetForm();
    }

    private MemberFormModel CollectFormModel()
    {
        return new MemberFormModel
        {
            MemberNo = _txtMemberNo.Text.Trim(),
            FirstName = _txtFirstName.Text.Trim(),
            MiddleName = _txtMiddleName.Text.Trim(),
            LastName = _txtLastName.Text.Trim(),
            MobileNo = _txtMobile.Text.Trim(),
            EmailId = _txtEmail.Text.Trim(),
            DOB = _dtDob.Value.Date,
            Age = int.TryParse(_txtAge.Text, out var age) ? age : 0,
            GenderId = 1,
            Address = _txtAddress.Text.Trim(),
            JoiningDate = _dtJoining.Value.Date,
            Status = true,
            EmergencyContactName = _txtEmergencyContactName.Text.Trim(),
            EmergencyContactNo = _txtEmergencyContactNo.Text.Trim(),
            MembershipTypeId = GetSelectedLookupId(_cmbMembershipType),
            WorkoutId = GetSelectedLookupId(_cmbWorkout),
            InstallmentId = GetSelectedLookupId(_cmbInstallment),
            PaymentTypeId = 1,
            TaxId = GetSelectedLookupId(_cmbTax)
        };
    }

    private void UpdateAge()
    {
        _txtAge.Text = _calculationService.CalculateAge(_dtDob.Value.Date).ToString();
    }

    private void RefreshAmount()
    {
        var membershipTypeId = GetSelectedLookupId(_cmbMembershipType);
        var taxId = GetSelectedLookupId(_cmbTax);

        if (membershipTypeId <= 0 || taxId <= 0)
        {
            _lblAmount.Text = "Amount: --";
            _lblTax.Text = "Tax: --";
            _lblTotal.Text = "Total: --";
            return;
        }

        var amount = _repository.GetMembershipAmount(membershipTypeId);
        var tax = _repository.GetTaxPercentage(taxId);
        var calculation = _calculationService.CalculateTotal(amount, tax);
        _lblAmount.Text = $"Amount: {calculation.Amount:0.00}";
        _lblTax.Text = $"Tax ({calculation.TaxPercentage:0.##}%): {calculation.TaxAmount:0.00}";
        _lblTotal.Text = $"Total: {calculation.TotalAmount:0.00}";
    }

    private void ResetForm()
    {
        _txtMemberNo.Text = _repository.GenerateMemberNo();
        _txtFirstName.Clear();
        _txtMiddleName.Clear();
        _txtLastName.Clear();
        _txtMobile.Clear();
        _txtEmail.Clear();
        _txtAddress.Text = "N/A";
        _txtEmergencyContactName.Clear();
        _txtEmergencyContactNo.Clear();
        _dtDob.Value = DateTime.Today.AddYears(-18);
        _dtJoining.Value = DateTime.Today;
        _cmbMembershipType.SelectedIndex = -1;
        _cmbWorkout.SelectedIndex = -1;
        _cmbInstallment.SelectedIndex = -1;
        _cmbTax.SelectedIndex = -1;
        UpdateAge();
        RefreshAmount();
    }

    private static void BindLookup(ComboBox comboBox, List<LookupItem> items)
    {
        comboBox.DataSource = items;
        comboBox.DisplayMember = nameof(LookupItem.Name);
        comboBox.ValueMember = nameof(LookupItem.Id);
        comboBox.SelectedIndex = -1;
        comboBox.Width = 230;
    }

    private static int GetSelectedLookupId(ComboBox comboBox)
    {
        return comboBox.SelectedValue is int value ? value : 0;
    }

    private static string UpperFirst(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        return char.ToUpperInvariant(value[0]) + value[1..].ToLowerInvariant();
    }

    private static void AddField(TableLayoutPanel panel, string labelText, Control inputControl, int row)
    {
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));
        var label = new Label
        {
            Text = labelText,
            AutoSize = true,
            Anchor = AnchorStyles.Left,
            Margin = new Padding(0, 8, 0, 0)
        };
        inputControl.Width = 230;
        panel.Controls.Add(label, row % 2 == 0 ? 0 : 2, row);
        panel.Controls.Add(inputControl, row % 2 == 0 ? 1 : 3, row);
    }
}
