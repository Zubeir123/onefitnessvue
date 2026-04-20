using FitnessTimeGym.Model.PaymentDetails;
using FitnessTimeGym.WinForms.Data;
using FitnessTimeGym.WinForms.Models;

namespace FitnessTimeGym.WinForms.Forms;

public class PaymentForm : Form
{
    private readonly GymRepository _repository;
    private readonly int _userId;
    private readonly CalculationService _calculationService = new();

    private readonly TextBox _txtMemberNo = new() { Width = 220 };
    private readonly Button _btnLoadMember = new() { Text = "Load Member", Width = 130, Height = 32 };
    private readonly Label _lblMemberName = new() { AutoSize = true, Text = "Member: --" };
    private readonly ComboBox _cmbMembershipType = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly ComboBox _cmbWorkout = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly ComboBox _cmbInstallment = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly ComboBox _cmbPaymentType = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly ComboBox _cmbTax = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly DateTimePicker _dtFromDate = new() { Format = DateTimePickerFormat.Short };
    private readonly Label _lblAmount = new() { AutoSize = true };
    private readonly Label _lblTax = new() { AutoSize = true };
    private readonly Label _lblTotal = new() { AutoSize = true };
    private readonly Button _btnSave = new() { Text = "Save Payment", Width = 140, Height = 36 };

    private long _memberId;
    private string _memberNo = string.Empty;

    public PaymentForm(GymRepository repository, int userId)
    {
        _repository = repository;
        _userId = userId;
        InitializeLayout();
        LoadLookups();
    }

    private void InitializeLayout()
    {
        Text = "Payment";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(820, 560);

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            Padding = new Padding(20)
        };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        AddField(root, "Member No", BuildMemberSearchPanel(), 0);
        AddField(root, "Member Name", _lblMemberName, 1);
        AddField(root, "Membership Type", _cmbMembershipType, 2);
        AddField(root, "Workout", _cmbWorkout, 3);
        AddField(root, "Installment", _cmbInstallment, 4);
        AddField(root, "Payment Type", _cmbPaymentType, 5);
        AddField(root, "Tax", _cmbTax, 6);
        AddField(root, "Payment From", _dtFromDate, 7);
        AddField(root, "Amount", _lblAmount, 8);
        AddField(root, "Tax Amount", _lblTax, 9);
        AddField(root, "Total", _lblTotal, 10);

        root.Controls.Add(_btnSave, 1, 11);

        _btnLoadMember.Click += OnLoadMemberClick;
        _cmbMembershipType.SelectedIndexChanged += (_, _) => RefreshAmount();
        _cmbTax.SelectedIndexChanged += (_, _) => RefreshAmount();
        _btnSave.Click += OnSavePaymentClick;

        Controls.Add(root);
    }

    private void LoadLookups()
    {
        BindLookup(_cmbMembershipType, _repository.GetMembershipTypes());
        BindLookup(_cmbWorkout, _repository.GetWorkouts());
        BindLookup(_cmbInstallment, _repository.GetInstallments());
        BindLookup(_cmbPaymentType, _repository.GetPaymentTypes());
        BindLookup(_cmbTax, _repository.GetTaxTypes());
        _dtFromDate.Value = DateTime.Today;
        RefreshAmount();
    }

    private Panel BuildMemberSearchPanel()
    {
        var panel = new Panel { Width = 520, Height = 36 };
        _txtMemberNo.Location = new Point(0, 0);
        _btnLoadMember.Location = new Point(235, 0);
        panel.Controls.Add(_txtMemberNo);
        panel.Controls.Add(_btnLoadMember);
        return panel;
    }

    private void OnLoadMemberClick(object? sender, EventArgs e)
    {
        var memberNo = _txtMemberNo.Text.Trim();
        if (string.IsNullOrWhiteSpace(memberNo))
        {
            MessageBox.Show("Enter member number.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var row = _repository.GetMemberByMemberNo(memberNo);
        if (row == null)
        {
            MessageBox.Show("Member not found.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _memberId = Convert.ToInt64(row["MemberId"]);
        _memberNo = Convert.ToString(row["MemberNo"]) ?? string.Empty;
        var fullName =
            $"{Convert.ToString(row["FirstName"])} {Convert.ToString(row["MiddleName"])} {Convert.ToString(row["LastName"])}".Trim();
        _lblMemberName.Text = $"Member: {fullName}";
    }

    private void OnSavePaymentClick(object? sender, EventArgs e)
    {
        if (_memberId <= 0 || string.IsNullOrWhiteSpace(_memberNo))
        {
            MessageBox.Show("Load a valid member first.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var membershipTypeId = GetSelectedLookupId(_cmbMembershipType);
        var workoutId = GetSelectedLookupId(_cmbWorkout);
        var installmentId = GetSelectedLookupId(_cmbInstallment);
        var paymentTypeId = GetSelectedLookupId(_cmbPaymentType);
        var taxId = GetSelectedLookupId(_cmbTax);

        if (membershipTypeId <= 0 || workoutId <= 0 || installmentId <= 0 || paymentTypeId <= 0 || taxId <= 0)
        {
            MessageBox.Show("Fill all payment fields.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var amount = _repository.GetMembershipAmount(membershipTypeId);
        var taxPercentage = _repository.GetTaxPercentage(taxId);
        var calculated = _calculationService.CalculateTotal(amount, taxPercentage);
        var installmentMonths = _repository.GetInstallmentMonths(installmentId);
        var toDate = _dtFromDate.Value.Date.AddMonths(installmentMonths);

        var payment = new PaymentDetailsModel
        {
            MemberID = _memberId,
            MemberNo = _memberNo,
            MembershipTypeId = membershipTypeId,
            WorkOutId = workoutId,
            InstallmentId = installmentId,
            PaymentTypeId = paymentTypeId,
            TaxId = taxId,
            Amount = calculated.Amount,
            TaxPercentage = calculated.TaxPercentage,
            TaxPercentageAmount = calculated.TaxAmount,
            TotalAmount = calculated.TotalAmount,
            PaymentFromdt = _dtFromDate.Value.Date,
            PaymentTodt = toDate,
            NextRenewalDate = toDate,
            RecStatus = "A",
            CreatedBy = _userId,
            CreatedOn = DateTime.Now,
            ApplicationType = "RE",
            InvoiceNo = _repository.GetNextInvoiceNo()
        };

        var saved = _repository.AddPayment(payment);
        if (!saved)
        {
            MessageBox.Show("Unable to save payment.", "Save Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        MessageBox.Show("Payment saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void RefreshAmount()
    {
        var membershipTypeId = GetSelectedLookupId(_cmbMembershipType);
        var taxId = GetSelectedLookupId(_cmbTax);
        if (membershipTypeId <= 0 || taxId <= 0)
        {
            _lblAmount.Text = "--";
            _lblTax.Text = "--";
            _lblTotal.Text = "--";
            return;
        }

        var amount = _repository.GetMembershipAmount(membershipTypeId);
        var tax = _repository.GetTaxPercentage(taxId);
        var calculated = _calculationService.CalculateTotal(amount, tax);
        _lblAmount.Text = $"{calculated.Amount:0.00}";
        _lblTax.Text = $"{calculated.TaxAmount:0.00}";
        _lblTotal.Text = $"{calculated.TotalAmount:0.00}";
    }

    private static void BindLookup(ComboBox comboBox, List<LookupItem> items)
    {
        comboBox.DataSource = items;
        comboBox.DisplayMember = nameof(LookupItem.Name);
        comboBox.ValueMember = nameof(LookupItem.Id);
        comboBox.SelectedIndex = -1;
        comboBox.Width = 250;
    }

    private static int GetSelectedLookupId(ComboBox comboBox)
    {
        return comboBox.SelectedValue is int value ? value : 0;
    }

    private static void AddField(TableLayoutPanel tableLayout, string labelText, Control control, int row)
    {
        tableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
        var label = new Label
        {
            Text = labelText,
            AutoSize = true,
            Anchor = AnchorStyles.Left
        };
        control.Anchor = AnchorStyles.Left;
        tableLayout.Controls.Add(label, 0, row);
        tableLayout.Controls.Add(control, 1, row);
    }
}
