using System.Text.Json;
using FitnessTimeGym.WinForms.Data;
using FitnessTimeGym.WinForms.Forms;

namespace FitnessTimeGym.WinForms;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        var connectionString = LoadConnectionString();
        var databaseHelper = new DatabaseHelper(connectionString);
        var repository = new GymRepository(databaseHelper);
        var authService = new AuthenticationService(repository);

        Application.Run(new LoginForm(authService, repository));
    }

    private static string LoadConnectionString()
    {
        var configFile = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        if (!File.Exists(configFile))
        {
            throw new FileNotFoundException("Missing appsettings.json for WinForms app.");
        }

        var content = File.ReadAllText(configFile);
        using var document = JsonDocument.Parse(content);
        if (!document.RootElement.TryGetProperty("ConnectionStrings", out var connectionStrings))
        {
            throw new InvalidOperationException("ConnectionStrings section is missing in appsettings.json.");
        }

        if (!connectionStrings.TryGetProperty("DatabaseConnection", out var connectionString))
        {
            throw new InvalidOperationException("DatabaseConnection is missing in appsettings.json.");
        }

        return connectionString.GetString() ?? throw new InvalidOperationException("DatabaseConnection cannot be null.");
    }
}
