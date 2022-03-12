namespace Crm.Link.Api
{
    public interface IJWTAuthenticationConfig
    {
        string Issuer { get; }

        string Audience { get; }

        string SymmetricKey { get; }

        string CertificateThumbPrint { get; }

        TimeSpan TokenLifetime { get; }
    }

    public class JWTAuthenticationConfig : IJWTAuthenticationConfig
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string SymmetricKey { get; set; }

        public string CertificateThumbPrint { get; set; }

        public TimeSpan TokenLifetime { get; set; }
    }
}
