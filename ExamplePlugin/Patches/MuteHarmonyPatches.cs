using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HarmonyLib;

namespace LogMute.Patches
{
    // name is bad cuz it kept autofilling to actual harmony shit
    public static class MuteHarmonyPatches
    {
        public static List<Regex> LogMuteCustom { get; set; }
        public static bool RegexExclude(object content) => !LogMuteCustom.Any(x => x.IsMatch(content?.ToString() ?? "Null"));

        public static void Init()
        {
            PluginConfig.myConfig.ConfigReloaded += ConfigReloaded;

            ConfigReloaded(null, null);

            var harm = new Harmony(LogMutePlugin.PluginGUID);

            harm.PatchAll(typeof(ScaryClassVanilla));
            harm.PatchAll(typeof(ScaryClassVanillaFormat));
            harm.PatchAll(typeof(ScaryClassVanillaException));
            harm.PatchAll(typeof(ScaryClassModded));
        }

        private static void ConfigReloaded(object _, EventArgs __) => LogMuteCustom =
        [
            .. CustomSplit(PluginConfig.exactMatch.Value).Select(x => new Regex("^\\s*" + x.Trim() + "\\s*$")),
            .. CustomSplit(PluginConfig.prefixMatch.Value).Select(x => new Regex("^\\s*" + x.Trim())),
            .. CustomSplit(PluginConfig.infixMatch.Value).Select(x => new Regex(x.Trim()))
        ];

        private static string[] CustomSplit(string str)
        {
            List<string> res = [];
            var txt = string.Empty;

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == ',')
                {
                    res.Add(txt);
                    txt = "";
                }
                else if (i + 1 < str.Length && str[i] == '\\' && str[i + 1] == ',')
                {
                    txt += ',';
                    i++;
                }
                else if (i + 1 < str.Length && str[i] == '\\' && str[i + 1] == '\\')
                {
                    txt += "\\\\";
                    i++;
                }
                else txt += str[i];
            }

            res.Add(txt);
            return [.. res.Where(x => !string.IsNullOrWhiteSpace(x))];
        }
    }
}
