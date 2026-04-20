using FitnessTimeGym.Common;
using FitnessTimeGym.WinForms.Data;

namespace FitnessTimeGym.WinForms.Forms;

public class LoginForm : Form
{
    private readonly AuthenticationService _authenticationService;
    private readonly GymRepository _repository;

    private readonly TextBox _txtUsername = new() { PlaceholderText = "Username", Width = 280 };
    private readonly TextBox _txtPassword = new() { PlaceholderText = "Password", Width = 280, UseSystemPasswordChar = true };
    private readonly Button _btnLogin = new() { Text = "Login", Width = 120, Height = 34 };
    private readonly Label _lblTitle = new() { Text = "Gym Management Login", AutoSize = true, Font = new Font("Segoe UI", 16, FontStyle.Bold) };
    private string _loginToken = string.Empty;

    public LoginForm(AuthenticationService authenticationService, GymRepository repository)
    {
        _authenticationService = authenticationService;
        _repository = repository;
        InitializeLayout();
        ResetToken();
    }

    private void InitializeLayout()
    {
        Text = "OneFitnessVue - Login";
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ClientSize = new Size(420, 260);

        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 5,
            Padding = new Padding(20)
        };

        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 56));
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 52));
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 52));
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 56));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        _btnLogin.Click += OnLoginClick;

        panel.Controls.Add(_lblTitle, 0, 0);
        panel.Controls.Add(_txtUsername, 0, 1);
        panel.Controls.Add(_txtPassword, 0, 2);
        panel.Controls.Add(_btnLogin, 0, 3);

        Controls.Add(panel);
        AcceptButton = _btnLogin;
    }

    private void OnLoginClick(object? sender, EventArgs e)
    {
        var username = _txtUsername.Text.Trim();
        var password = _txtPassword.Text.Trim();

        var passwordHash = HashHelper.CreateHashSHA256(password);
        var clientHash = HashHelper.CreateHashSHA256($"{_loginToken}{passwordHash}");
        var result = _authenticationService.Login(username, clientHash, _loginToken);

        if (!result.isSuccess || result.session == null)
        {
            MessageBox.Show(result.message, "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            ResetToken();
            return;
        }

        Hide();
        using var dashboard = new DashboardForm(result.session, _repository);
        dashboard.ShowDialog();
        Show();

        _txtPassword.Text = string.Empty;
        ResetToken();
    }

    private void ResetToken()
    {
        _loginToken = _repository.GenerateLoginToken();
    }
}
