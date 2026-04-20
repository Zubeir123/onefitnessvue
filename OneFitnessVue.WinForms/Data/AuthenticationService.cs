using FitnessTimeGym.Common;
using FitnessTimeGym.WinForms.Models;

namespace FitnessTimeGym.WinForms.Data;

public class AuthenticationService
{
    private readonly GymRepository _repository;

    public AuthenticationService(GymRepository repository)
    {
        _repository = repository;
    }

    public (bool isSuccess, string message, UserSession? session) Login(string username, string clientPasswordHash, string loginToken)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(clientPasswordHash))
        {
            return (false, "Username and password are required.", null);
        }

        if (!_repository.CheckUsernameExists(username))
        {
            return (false, "Entered Username or Password is Invalid", null);
        }

        var session = _repository.GetUserSession(username);
        if (session == null)
        {
            return (false, "Entered Username or Password is Invalid", null);
        }

        if (!session.Status)
        {
            return (false, "Your Account is InActive Contact Administrator", null);
        }

        var storedPasswordHash = _repository.GetPasswordHash(username);
        if (string.IsNullOrWhiteSpace(storedPasswordHash))
        {
            return (false, "Entered Username or Password is Invalid", null);
        }

        var expectedHash = HashHelper.CreateHashSHA256($"{loginToken}{storedPasswordHash}");
        return expectedHash.Equals(clientPasswordHash, StringComparison.OrdinalIgnoreCase)
            ? (true, string.Empty, session)
            : (false, "Entered Username or Password is Invalid", null);
    }
}
