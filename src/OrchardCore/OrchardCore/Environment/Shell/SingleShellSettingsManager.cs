using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace OrchardCore.Environment.Shell
{
    public class SingleShellSettingsManager : IShellSettingsManager
    {
        public IEnumerable<ShellSettings> LoadSettings()
        {
            yield return new ShellSettings
            {
                Name = "Default",
                State = Models.TenantState.Running.ToString()
            };
        }

        public void SaveSettings(ShellSettings shellSettings)
        {
        }

        public IConfiguration Configuration => throw new System.NotImplementedException();
    }
}