using FitnessTimeGym.WinForms.Data;

namespace FitnessTimeGym.WinForms.Forms;

public class ReportsForm : Form
{
    private readonly GymRepository _repository;

    private readonly DateTimePicker _dtFrom = new() { Format = DateTimePickerFormat.Short };
    private readonly DateTimePicker _dtTo = new() { Format = DateTimePickerFormat.Short };
    private readonly Button _btnLoadRenewal = new() { Text = "Load Renewal Report", Width = 170, Height = 32 };
    private readonly DataGridView _renewalGrid = new() { Dock = DockStyle.Fill, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };

    private readonly TextBox _txtPaymentSearch = new() { PlaceholderText = "Search payments by member no/name", Width = 300 };
    private readonly Button _btnPaymentSearch = new() { Text = "Search", Width = 100, Height = 32 };
    private readonly DataGridView _paymentGrid = new() { Dock = DockStyle.Fill, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };

    public ReportsForm(GymRepository repository)
    {
        _repository = repository;
        InitializeLayout();
        LoadPaymentHistory();
    }

    private void InitializeLayout()
    {
        Text = "Reports";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(1080, 640);

        var tabs = new TabControl { Dock = DockStyle.Fill };
        tabs.TabPages.Add(BuildRenewalPage());
        tabs.TabPages.Add(BuildPaymentHistoryPage());

        _dtFrom.Value = DateTime.Today.AddDays(-30);
        _dtTo.Value = DateTime.Today;
        _btnLoadRenewal.Click += (_, _) => LoadRenewalReport();
        _btnPaymentSearch.Click += (_, _) => LoadPaymentHistory();

        Controls.Add(tabs);
    }

    private TabPage BuildRenewalPage()
    {
        var page = new TabPage("Renewal Report");
        var topPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 56,
            Padding = new Padding(10)
        };
        topPanel.Controls.Add(new Label { Text = "From", AutoSize = true, Margin = new Padding(0, 8, 8, 0) });
        topPanel.Controls.Add(_dtFrom);
        topPanel.Controls.Add(new Label { Text = "To", AutoSize = true, Margin = new Padding(18, 8, 8, 0) });
        topPanel.Controls.Add(_dtTo);
        topPanel.Controls.Add(_btnLoadRenewal);

        page.Controls.Add(_renewalGrid);
        page.Controls.Add(topPanel);
        return page;
    }

    private TabPage BuildPaymentHistoryPage()
    {
        var page = new TabPage("Payment History");
        var topPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 56,
            Padding = new Padding(10)
        };
        topPanel.Controls.Add(_txtPaymentSearch);
        topPanel.Controls.Add(_btnPaymentSearch);

        page.Controls.Add(_paymentGrid);
        page.Controls.Add(topPanel);
        return page;
    }

    private void LoadRenewalReport()
    {
        if (_dtTo.Value.Date < _dtFrom.Value.Date)
        {
            MessageBox.Show("To date should be greater than or equal to from date.", "Validation", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        _renewalGrid.DataSource = _repository.GetRenewalReport(_dtFrom.Value.Date, _dtTo.Value.Date);
    }

    private void LoadPaymentHistory()
    {
        _paymentGrid.DataSource = _repository.SearchPayments(_txtPaymentSearch.Text.Trim());
    }
}
