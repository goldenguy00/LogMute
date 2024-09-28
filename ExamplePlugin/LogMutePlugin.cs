using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using static RoR2.Console;

namespace LogMute
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class LogMutePlugin : BaseUnityPlugin
    {
        public const string PluginGUID = "_" + PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "score";
        public const string PluginName = "LogMute";
        public const string PluginVersion = "1.0.0";
        public static Harmony Harmony;
        public static ConfigFile Config;
        public static List<Regex> LogMuteCustom = [];
        public static bool RegexExclude(string content) => !LogMuteCustom.Any(x => x.IsMatch(content));

        private static void Fuck(object you) { }
        private static void FuckUnity(object you, Object fuck) { }
        private static void FuckYou(string fuck, object[] you) { }
        private static void ExtraFuckYou(Object extra, string fuck, object[] you) { }

        public void Awake()
        {
            Harmony = new(PluginGUID);
            Config = new(System.IO.Path.Combine(Paths.ConfigPath, PluginGUID + ".cfg"), true);
            var _LogMuteExact = Config.Bind("General", "Exact Matches to Filter", "Teambuff", "List of exact matches to filter, separated by comma. accepts regex patterns.");
            var _LogMuteStartWith = Config.Bind("General", "Prefix Matches to Filter", "", "List of prefix matches to filter, separated by comma. accepts regex patterns.");
            var _LogMuteInclude = Config.Bind("General", "Infix Matches to Filter", "", "List of infix matches to filter, separated by comma. accepts regex patterns.");
            LogMuteCustom.AddRange(_LogMuteExact.Value.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => new Regex("^\\s*" + x.Trim() + "\\s*$")));
            LogMuteCustom.AddRange(_LogMuteStartWith.Value.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => new Regex("^\\s*" + x.Trim())));
            LogMuteCustom.AddRange(_LogMuteInclude.Value.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => new Regex(x.Trim())));
            Harmony.PatchAll(typeof(ScaryClassVanilla));
            Harmony.PatchAll(typeof(ScaryClassVanillaFormat));
            Harmony.PatchAll(typeof(ScaryClassVanillaException));
            Harmony.PatchAll(typeof(ScaryClassModded));
            // vanilla patches
            IL.RoR2.EffectComponent.Start += EffectComponent_Start;
            IL.RoR2.EffectManagerHelper.Reset += EffectManagerHelper_Reset;
            IL.RoR2.EffectManagerHelper.StopAllParticleSystems += EffectManagerHelper_StopAllParticleSystems;
            IL.RoR2.Tracer.PrepForPoolUsage += Tracer_PrepForPoolUsage;
            IL.RoR2.EffectManager.SpawnEffect_GameObject_EffectData_bool += EffectManager_SpawnEffect_GameObject_EffectData_bool;
            IL.RoR2.UI.InputSourceFilter.Refresh += InputSourceFilter_Refresh;

            new ILHook(AccessTools.DeclaredMethod(typeof(NetworkScene), nameof(NetworkScene.RegisterPrefab), [typeof(GameObject)]), NoRegister);
            new ILHook(AccessTools.DeclaredMethod(typeof(NetworkScene), nameof(NetworkScene.RegisterPrefab), [typeof(GameObject), typeof(NetworkHash128)]), NoRegister2);
            new ILHook(AccessTools.DeclaredMethod(typeof(NetworkScene), nameof(NetworkScene.RegisterSpawnHandler)), NoRegister3);
            new ILHook(AccessTools.DeclaredPropertySetter(typeof(Transform), nameof(Transform.parent)), NoParent);
            new ILHook(AccessTools.DeclaredPropertyGetter(typeof(ItemDef), nameof(ItemDef.itemIndex)), NoFix);
            new ILHook(AccessTools.DeclaredPropertyGetter(typeof(ItemTierDef), nameof(ItemTierDef.tier)), NoFix);
            new ILHook(AccessTools.DeclaredPropertyGetter(typeof(EquipmentDef), nameof(EquipmentDef.equipmentIndex)), NoFix);
        }

        private static void NoRegister(ILContext il)
        {
            var c = new ILCursor(il);

            if (c.TryGotoNext(x => x.MatchCallOrCallvirt<Debug>(nameof(Debug.Log))))
            {
                c.Remove();
                c.EmitDelegate(Fuck);
            }

            if (c.TryGotoNext(x => x.MatchCallOrCallvirt<Debug>(nameof(Debug.LogWarning))))
            {
                c.Remove();
                c.EmitDelegate(Fuck);
            }

            if (c.TryGotoNext(x => x.MatchCallOrCallvirt<Debug>(nameof(Debug.LogError))))
            {
                c.Remove();
                c.EmitDelegate(Fuck);
            }
        }
        private static void NoRegister2(ILContext il)
        {
            var c = new ILCursor(il);

            if (c.TryGotoNext(x => x.MatchCallOrCallvirt<Debug>(nameof(Debug.Log))))
            {
                c.Remove();
                c.EmitDelegate(Fuck);
            }

            if (c.TryGotoNext(x => x.MatchCallOrCallvirt<Debug>(nameof(Debug.LogError))))
            {
                c.Remove();
                c.EmitDelegate(Fuck);
            }
        }

        private static void NoRegister3(ILContext il)
        {
            var c = new ILCursor(il);

            if (c.TryGotoNext(x => x.MatchCallOrCallvirt<Debug>(nameof(Debug.LogError))))
            {
                c.Remove();
                c.EmitDelegate(Fuck);
            }

            if (c.TryGotoNext(x => x.MatchCallOrCallvirt<Debug>(nameof(Debug.LogError))))
            {
                c.Remove();
                c.EmitDelegate(Fuck);
            }

            if (c.TryGotoNext(x => x.MatchCallOrCallvirt<Debug>(nameof(Debug.LogError))))
            {
                c.Remove();
                c.EmitDelegate(Fuck);
            }

            if (c.TryGotoNext(x => x.MatchCallOrCallvirt<Debug>(nameof(Debug.Log))))
            {
                c.Remove();
                c.EmitDelegate(Fuck);
            }
        }

        private static void NoFix(ILContext il)
        {
            var c = new ILCursor(il);

            if (c.TryGotoNext(x => x.MatchCallOrCallvirt<Debug>(nameof(Debug.LogError))))
            {
                c.Remove();
                c.EmitDelegate(Fuck);
            }
            if (c.TryGotoNext(x => x.MatchCallOrCallvirt<Debug>(nameof(Debug.LogError))))
            {
                c.Remove();
                c.EmitDelegate(Fuck);
            }
        }

        private static void NoParent(ILContext il)
        {
            var c = new ILCursor(il);

            if (c.TryGotoNext(x => x.MatchCallOrCallvirt<Debug>(nameof(Debug.LogWarning))))
            {
                c.Remove();
                c.EmitDelegate(FuckUnity);
            }
        }

        private static void InputSourceFilter_Refresh(ILContext il)
        {
            var c = new ILCursor(il);

            if (c.TryGotoNext(x => x.MatchCallOrCallvirt<Debug>(nameof(Debug.LogWarningFormat))))
            {
                c.Remove();
                c.EmitDelegate(FuckYou);
            }
        }

        private static void EffectManager_SpawnEffect_GameObject_EffectData_bool(ILContext il)
        {
            var c = new ILCursor(il);

            if (c.TryGotoNext(x => x.MatchCallOrCallvirt<Debug>(nameof(Debug.LogError))))
            {
                c.Remove();
                c.EmitDelegate(Fuck);
            }
        }


        private static void Tracer_PrepForPoolUsage(ILContext il)
        {
            var c = new ILCursor(il);

            if (c.TryGotoNext(x => x.MatchCallOrCallvirt<Debug>(nameof(Debug.LogFormat))))
            {
                c.Remove();
                c.EmitDelegate(FuckYou);
            }

            if (c.TryGotoNext(x => x.MatchCallOrCallvirt<Debug>(nameof(Debug.LogFormat))))
            {
                c.Remove();
                c.EmitDelegate(FuckYou);
            }
        }

        private static void EffectManagerHelper_StopAllParticleSystems(ILContext il)
        {
            var c = new ILCursor(il);

            if (c.TryGotoNext(x => x.MatchCallOrCallvirt<Debug>(nameof(Debug.LogFormat))))
            {
                c.Remove();
                c.EmitDelegate(FuckYou);
            }
        }

        private static void EffectManagerHelper_Reset(ILContext il)
        {
            var c = new ILCursor(il);

            if (c.TryGotoNext(x => x.MatchCallOrCallvirt<Debug>(nameof(Debug.LogFormat))))
            {
                c.Remove();
                c.EmitDelegate(FuckYou);
            }
        }

        private static void EffectComponent_Start(ILContext il)
        {
            var c = new ILCursor(il);

            if (c.TryGotoNext(x => x.MatchCallOrCallvirt<Debug>(nameof(Debug.LogErrorFormat))))
            {
                c.Remove();
                c.EmitDelegate(ExtraFuckYou);
            }
        }
    }
}
