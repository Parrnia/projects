namespace Onyx.Application.Settings;

public class JwtTokenSettings
{
    public string AuthenticationAddress { get; set; }
    public string ValidationKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}
