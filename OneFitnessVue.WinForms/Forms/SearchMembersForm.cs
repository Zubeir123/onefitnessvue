using FitnessTimeGym.WinForms.Data;

namespace FitnessTimeGym.WinForms.Forms;

public class SearchMembersForm : Form
{
    private readonly GymRepository _repository;
    private readonly TextBox _txtSearch = new() { PlaceholderText = "Search by Member No or First Name", Width = 360 };
    private readonly Button _btnSearch = new() { Text = "Search", Width = 100, Height = 32 };
    private readonly DataGridView _grid = new() { Dock = DockStyle.Fill, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };

    public SearchMembersForm(GymRepository repository)
    {
        _repository = repository;
        InitializeLayout();
        LoadMembers();
    }

    private void InitializeLayout()
    {
        Text = "Search Members";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(980, 600);

        var topPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 56,
            Padding = new Padding(12, 12, 12, 12)
        };
        _btnSearch.Click += (_, _) => LoadMembers();
        _txtSearch.KeyDown += (_, e) =>
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadMembers();
            }
        };

        topPanel.Controls.Add(_txtSearch);
        topPanel.Controls.Add(_btnSearch);

        Controls.Add(_grid);
        Controls.Add(topPanel);
    }

    private void LoadMembers()
    {
        _grid.DataSource = _repository.SearchMembers(_txtSearch.Text.Trim());
    }
}
