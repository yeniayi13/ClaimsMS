using System.Diagnostics.CodeAnalysis;

namespace ClaimsMS.Infrastructure.Settings
{
    public class AppSettings
    {
        [ExcludeFromCodeCoverage]

        public string? key1 { get; set; }
    }
}
