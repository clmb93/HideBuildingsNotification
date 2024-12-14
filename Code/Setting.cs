using System.Collections.Generic;
using Colossal;
using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;

namespace HideBuildingNotification
{
    [FileLocation("ModsSettings/HideBuildingNotification/HideBuildingNotification.coc")]
    [SettingsUIGroupOrder(kToggleGroup)]
    [SettingsUIShowGroupName(kToggleGroup)]

    public class Setting : ModSetting
    {
        public const string kSection = "Main";
        public const string kToggleGroup = "Configuration";


        [SettingsUISection(kSection, kToggleGroup)]
        public bool hideBuldingNotifications { get; set; } = true;

        [SettingsUISection(kSection, kToggleGroup)]
        public string version
        {
            get => "1.0.0";
        }

        public override void SetDefaults()
        {
        }

        public Setting(IMod mod) : base(mod)
        {
        }
    }

    public class LocaleEN : IDictionarySource
    {
        private readonly Setting m_Setting;
        public LocaleEN(Setting setting)
        {
            m_Setting = setting;


        }

        public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts)
        {
            return new Dictionary<string, string>
            {
                //Menu ui
                { m_Setting.GetSettingsLocaleID(), "Hide buildings notifications" },
                { m_Setting.GetOptionTabLocaleID(Setting.kSection), "Main" },
                { m_Setting.GetOptionGroupLocaleID(Setting.kToggleGroup), "Configuration" },              
                
                //Building need electricity
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.hideBuldingNotifications)), "Hide building notifications" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.hideBuldingNotifications)), $"Set true to hide building notifications" },

                //Version
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.version)), "Version" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.version)), $"Current version of the mod" },

            };
        }

        public void Unload() { }

    }
}
