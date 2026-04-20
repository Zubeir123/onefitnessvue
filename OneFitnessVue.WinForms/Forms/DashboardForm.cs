using FitnessTimeGym.WinForms.Data;
using FitnessTimeGym.WinForms.Models;

namespace FitnessTimeGym.WinForms.Forms;

public class DashboardForm : Form
{
    private readonly UserSession _session;
    private readonly GymRepository _repository;

    public DashboardForm(UserSession session, GymRepository repository)
    {
        _session = session;
        _repository = repository;
        InitializeLayout();
    }

    private void InitializeLayout()
    {
        Text = $"OneFitnessVue Dashboard - {_session.UserName}";
        WindowState = FormWindowState.Maximized;
        StartPosition = FormStartPosition.CenterScreen;

        var container = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            Padding = new Padding(24)
        };
        container.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250));
        container.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        var navPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            Padding = new Padding(10)
        };

        var contentPanel = new Panel
        {
            Dock = DockStyle.Fill
        };

        var lblWelcome = new Label
        {
            Text = $"Welcome, {_session.FirstName}",
            AutoSize = true,
            Font = new Font("Segoe UI", 18, FontStyle.Bold),
            Location = new Point(15, 20)
        };
        contentPanel.Controls.Add(lblWelcome);

        navPanel.Controls.Add(CreateNavButton("Add Member", (_, _) => new AddMemberForm(_repository, _session.UserId).ShowDialog(this)));
        navPanel.Controls.Add(CreateNavButton("Search Member", (_, _) => new SearchMembersForm(_repository).ShowDialog(this)));
        navPanel.Controls.Add(CreateNavButton("Payment", (_, _) => new PaymentForm(_repository, _session.UserId).ShowDialog(this)));
        navPanel.Controls.Add(CreateNavButton("Reports", (_, _) => new ReportsForm(_repository).ShowDialog(this)));
        navPanel.Controls.Add(CreateNavButton("Logout", (_, _) => Close()));

        container.Controls.Add(navPanel, 0, 0);
        container.Controls.Add(contentPanel, 1, 0);
        Controls.Add(container);
    }

    private static Button CreateNavButton(string text, EventHandler clickHandler)
    {
        var button = new Button
        {
            Text = text,
            Width = 200,
            Height = 44,
            Margin = new Padding(0, 0, 0, 12)
        };
        button.Click += clickHandler;
        return button;
    }
}
