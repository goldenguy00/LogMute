using BepInEx;
using HarmonyLib;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace LogMute
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class LogMutePlugin : BaseUnityPlugin
    {
        public const string PluginGUID = "_" + PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "score";
        public const string PluginName = "LogMute";
        public const string PluginVersion = "1.0.0";

        private static void Fuck(object you) { }
        private static void FuckUnity(object you, Object fuck) { }
        private static void FuckYou(string fuck, object[] you) { }
        private static void ExtraFuckYou(Object extra, string fuck, object[] you) { }

        public void Awake()
        {
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
