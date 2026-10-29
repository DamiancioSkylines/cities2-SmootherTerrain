using System.Collections.Generic;
using Colossal;

namespace SmootherTerrain.Settings
{
    public class LocaleEn : IDictionarySource
    {
        // ReSharper disable once InconsistentNaming
        private readonly Setting Setting;

        public LocaleEn(Setting setting)
        {
            Setting = setting;
        }
        
        public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts)
        {
            return new Dictionary<string, string>
            {
                // Mod Name and Tab Name
                { Setting.GetSettingsLocaleID(), "Smoother Terrain" },
                { Setting.GetOptionTabLocaleID(Setting.MainTab), "Main" },
                
                // Group Labels
                { Setting.GetOptionGroupLocaleID(Setting.SettingsGroup), "Smoothing Terrain Settings" },
                { Setting.GetOptionGroupLocaleID(Setting.LinksGroup), "Links" },
                
                // Smoothing Factor
                { Setting.GetOptionLabelLocaleID(nameof(Setting.SmoothingFactor)), "Smoothing Factor" },
                { Setting.GetOptionDescLocaleID(nameof(Setting.SmoothingFactor)), "Increases sampling radius by a factor when calculating underlying terrain normals" },
                
                // Reset
                { Setting.GetOptionLabelLocaleID(nameof(Setting.ResetSettings)), "Reset" },
                { Setting.GetOptionDescLocaleID(nameof(Setting.ResetSettings)), "Reset settings of the mod to vanilla"},
                
                // GitHub button
                { Setting.GetOptionLabelLocaleID(nameof(Setting.GithubLink)), "Source Code on GitHub" },
                { Setting.GetOptionDescLocaleID(nameof(Setting.GithubLink)), "Access link to source code"},
            };
        }

        public void Unload() { }
    }
}