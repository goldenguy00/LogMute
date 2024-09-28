using System.Security.Permissions;
using System.Security;
using BepInEx;
using BepInEx.Bootstrap;
using LogMute.Patches;

[module: UnverifiableCode]
#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618 // Type or member is obsolete

namespace LogMute
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class LogMutePlugin : BaseUnityPlugin
    {
        public const string PluginGUID = "_" + PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "score";
        public const string PluginName = "LogMute";
        public const string PluginVersion = "1.0.1";

        public static bool RooInstalled => Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions");

        public static LogMutePlugin Instance { get; private set; }

        public void Awake()
        {
            Instance = this;

            PluginConfig.Init();
            VanillaPatches.Init();
            MuteHarmonyPatches.Init();
        }
    }
}
