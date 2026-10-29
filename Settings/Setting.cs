using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Game.Input;
using Game.UI.Widgets;
using UnityEngine;
using JetBrains.Annotations;

namespace SmootherTerrain.Settings
{
    /// <summary>
    /// Represents the mod's settings class, handling UI presentation, keybindings, and various mod-specific parameters.
    /// This class extends <see cref="ModSetting"/> to integrate with the game's settings menu.
    /// </summary>
    /// <remarks>
    /// This class defines UI groups, tab order, and keyboard/gamepad actions for various mod functionalities and general mod settings.
    /// </remarks>
    // Mod settings class, handling UI and keybindings.
    [FileLocation("ModsSettings/SmootherTerrain/" + nameof(SmootherTerrain))]
    // Define the order of groups within the tabs
    [SettingsUIGroupOrder(SettingsGroup,LinksGroup)]
    // Show group names
    [SettingsUIShowGroupName(SettingsGroup,LinksGroup)]
    // Define tab order e.g
    // [SettingsUITabOrder(MainTab, SecondTab)]
    
    public class Setting : ModSetting
    {
        public const string MainTab = "MainTab";
        public const string SettingsGroup = "SettingsGroup";
        public const string LinksGroup = "LinksGroup";
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Setting"/> class.
        /// </summary>
        /// <param name="mod">The mod instance associated with these settings.</param>
        public Setting(IMod mod) : base(mod) { }
        
        [SettingsUISection(MainTab, SettingsGroup)]
        [SettingsUISlider(min = 0.0f, max = 32.0f, step = 1.0f, unit = "Scale")]
        public float SmoothingFactor { get; set; }
        
        [SettingsUIButton]
        [SettingsUIButtonGroup("Reset")]
        [SettingsUISection(MainTab, SettingsGroup)]
        public bool ResetSettings
        {
            // ReSharper disable once ValueParameterNotUsed
            set => SetDefaults();
        }
        
        [SettingsUIButton]
        [SettingsUIButtonGroup("Links")]
        [SettingsUISection(MainTab, LinksGroup)]
        [UsedImplicitly]
        public bool GithubLink {
            // ReSharper disable once ValueParameterNotUsed
            set => Application.OpenURL("https://github.com/DamiancioSkylines/cities2-SmootherTerrain");
        }
        
        public override void SetDefaults()
        {
            SmoothingFactor = 0;
        }
    }
}