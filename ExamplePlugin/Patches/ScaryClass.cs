using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace LogMute.Patches
{
    [HarmonyPatch]
    public class ScaryClassVanilla
    {
        public static bool Prefix(ref object message) => MuteHarmonyPatches.RegexExclude(message);
        public static IEnumerable<MethodBase> TargetMethods() =>
        [
            typeof(Debug).GetMethod(nameof(Debug.Log), [typeof(object)]),
            typeof(Debug).GetMethod(nameof(Debug.Log), [typeof(object), typeof(Object)]),
            typeof(Debug).GetMethod(nameof(Debug.LogError), [typeof(object)]),
            typeof(Debug).GetMethod(nameof(Debug.LogError), [typeof(object), typeof(Object)]),
            typeof(Debug).GetMethod(nameof(Debug.LogWarning), [typeof(object)]),
            typeof(Debug).GetMethod(nameof(Debug.LogWarning), [typeof(object), typeof(Object)]),
        ];
    }

    [HarmonyPatch]
    public class ScaryClassVanillaFormat
    {
        public static bool Prefix(ref object format) => MuteHarmonyPatches.RegexExclude(format);
        public static IEnumerable<MethodBase> TargetMethods() =>
        [
            typeof(Debug).GetMethod(nameof(Debug.LogFormat), [typeof(string), typeof(object[])]),
            typeof(Debug).GetMethod(nameof(Debug.LogFormat), [typeof(Object), typeof(string), typeof(object[])]),
            typeof(Debug).GetMethod(nameof(Debug.LogErrorFormat), [typeof(string), typeof(object[])]),
            typeof(Debug).GetMethod(nameof(Debug.LogErrorFormat), [typeof(Object), typeof(string), typeof(object[])]),
            typeof(Debug).GetMethod(nameof(Debug.LogWarningFormat), [typeof(string), typeof(object[])]),
            typeof(Debug).GetMethod(nameof(Debug.LogWarningFormat), [typeof(Object), typeof(string), typeof(object[])]),
        ];
    }

    [HarmonyPatch]
    public class ScaryClassVanillaException
    {
        public static bool Prefix(ref System.Exception exception) => MuteHarmonyPatches.RegexExclude(exception);

        public static IEnumerable<MethodBase> TargetMethods() =>
        [
            typeof(Debug).GetMethod(nameof(Debug.LogException), [typeof(System.Exception)]),
            typeof(Debug).GetMethod(nameof(Debug.LogException), [typeof(System.Exception), typeof(Object)]),
        ];
    }

    [HarmonyPatch]
    public class ScaryClassModded
    {
        public static bool Prefix(ref object data) => MuteHarmonyPatches.RegexExclude(data);
        public static MethodBase TargetMethod() => typeof(ManualLogSource).GetMethod(nameof(ManualLogSource.Log), [typeof(LogLevel), typeof(object)]);
    }
}
