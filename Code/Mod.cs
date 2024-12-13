using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Game;
using Game.Modding;
using Game.SceneFlow;
using HideBuildingsNotification;

namespace HideBuildingNotification
{
    public class Mod : IMod
    {
        public static ILog log = LogManager.GetLogger($"{nameof(HideBuildingNotification)}").SetShowsErrorsInUI(false);
        private static bool m_HideBuldingNotification => m_Setting.hideBuldingNotifications;
        private static Setting m_Setting;

        public void OnLoad(UpdateSystem updateSystem)
        {
            log.Info("Mod is loaded");
            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
                log.Info($"Current mod asset at {asset.path}");

            m_Setting = new Setting(this);
            m_Setting.RegisterInOptionsUI();
            GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(m_Setting));
            AssetDatabase.global.LoadSettings(nameof(HideBuildingNotification), m_Setting, new Setting(this));
            updateSystem.UpdateBefore<DisableBuidlingNotificationsSystem>(SystemUpdatePhase.PreSimulation);
        }

        public void OnDispose()
        {
            log.Info(nameof(OnDispose));
            if (m_Setting != null)
            {
                m_Setting.UnregisterInOptionsUI();
                m_Setting = null;
            }
        }

        public static bool GetHideBuildingNotifications()
        {
            return m_HideBuldingNotification;
        }
    }
}
