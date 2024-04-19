using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using TicketReservation.Persistence.Clients;

namespace TicketReservation.WebApi.Auth;

public sealed class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private const string AuthHeaderName = "Authorization";
    private const string HeaderPrefix = "Basic ";

    private readonly IClientRepository _clientRepository;

    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IClientRepository clientRepository) : base(options, logger, encoder)
    {
        _clientRepository = clientRepository;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(AuthHeaderName, out var value))
        {
            return AuthenticateResult.Fail($"Header '{AuthHeaderName}' is not provider");
        }
        string? header = value;
        if (string.IsNullOrEmpty(header))
        {
            return AuthenticateResult.Fail("No value for header is provided");
        }
        if (!header.StartsWith(HeaderPrefix, StringComparison.OrdinalIgnoreCase))
        {
            return AuthenticateResult.Fail($"Header prefix '{HeaderPrefix}' is apsent");
        }
        string token = header[HeaderPrefix.Length..].Trim();
        string credentialsAsEncodedString = Encoding.UTF8.GetString(Convert.FromBase64String(token));
        string[] credentials = credentialsAsEncodedString.Split(':');
        if (credentials.Length != 2)
        {
            return AuthenticateResult.Fail("Invalid Authorization header format");
        }
        string username = credentials[0];
        string password = credentials[1];
        var clinet = await _clientRepository.FindByCredentials(username, password);
        if (clinet is null)
        {
            return AuthenticateResult.Fail("Authentication failed");
        }
        Claim[] claims = [
            new Claim(ClaimTypes.NameIdentifier, clinet.Id.ToString()),
            new Claim(ClaimTypes.Name, clinet.Name)];
        var identity = new ClaimsIdentity(claims, "Basic");
        var claimsPrincipal = new ClaimsPrincipal(identity);
        return AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name));
    }

    protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.StatusCode = StatusCodes.Status401Unauthorized;
        var problemDetails = new ProblemDetails
        {
            Status = Response.StatusCode,
            Title = "Unauthrorized",
            Type = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1",
            Instance = Context.TraceIdentifier
        };
        await Response.WriteAsJsonAsync(problemDetails);
    }
}
