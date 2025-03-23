namespace SafeguardSystem.Common.DTOs;

public class GoogleLoginDTO
{
    /// <summary>
    ///     The ID token received from Google after authentication.
    /// </summary>
    public required string IdToken { get; set; }
}