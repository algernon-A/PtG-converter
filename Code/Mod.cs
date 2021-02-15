using ICities;
using CitiesHarmony.API;


namespace PlopTheGrowables
{
    /// <summary>
    /// The base mod class for instantiation by the game.
    /// </summary>
    public class PtGReaderMod : IUserMod
    {
        public static string ModName => "Plop the Growables data converter ";
        public static string Version => "1.0.1";

        public string Name => ModName + " " + Version;
        public string Description => Translations.Translate("PGC_DESC");


        /// <summary>
        /// Called by the game when the mod is enabled.
        /// </summary>
        public void OnEnabled()
        {
            // Apply Harmony patches via Cities Harmony.
            // Called here instead of OnCreated to allow the auto-downloader to do its work prior to launch.
            HarmonyHelper.DoOnHarmonyReady(() => Patcher.PatchAll());
        }


        /// <summary>
        /// Called by the game when the mod is disabled.
        /// </summary>
        public void OnDisabled()
        {
            // Unapply Harmony patches via Cities Harmony.
            if (HarmonyHelper.IsHarmonyInstalled)
            {
                Patcher.UnpatchAll();
            }
        }
    }
}
