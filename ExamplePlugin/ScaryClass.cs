using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace LogMute
{

    [HarmonyPatch]
    public class ScaryClassVanilla
    {
        public static bool Prefix(ref object message)
        {
            if (message == null) message = "Null";
            return LogMutePlugin.RegexExclude(message.ToString());
        }
        public static IEnumerable<MethodBase> TargetMethods()
        {
            return [
                typeof(Debug).GetMethod(nameof(Debug.Log), [typeof(object)]),
                typeof(Debug).GetMethod(nameof(Debug.Log), [typeof(object), typeof(Object)]),
                typeof(Debug).GetMethod(nameof(Debug.LogError), [typeof(object)]),
                typeof(Debug).GetMethod(nameof(Debug.LogError), [typeof(object), typeof(Object)]),
                typeof(Debug).GetMethod(nameof(Debug.LogWarning), [typeof(object)]),
                typeof(Debug).GetMethod(nameof(Debug.LogWarning), [typeof(object), typeof(Object)]),
            ];
        }
    }

    [HarmonyPatch]
    public class ScaryClassVanillaFormat
    {
        public static bool Prefix(ref object format)
        {
            if (format == null) format = "Null";
            return LogMutePlugin.RegexExclude(format.ToString());
        }
        public static IEnumerable<MethodBase> TargetMethods()
        {
            return [
                typeof(Debug).GetMethod(nameof(Debug.LogFormat), [typeof(string), typeof(object[])]),
                typeof(Debug).GetMethod(nameof(Debug.LogFormat), [typeof(Object), typeof(string), typeof(object[])]),
                typeof(Debug).GetMethod(nameof(Debug.LogErrorFormat), [typeof(string), typeof(object[])]),
                typeof(Debug).GetMethod(nameof(Debug.LogErrorFormat), [typeof(Object), typeof(string), typeof(object[])]),
                typeof(Debug).GetMethod(nameof(Debug.LogWarningFormat), [typeof(string), typeof(object[])]),
                typeof(Debug).GetMethod(nameof(Debug.LogWarningFormat), [typeof(Object), typeof(string), typeof(object[])]),
            ];
        }
    }

    [HarmonyPatch]
    public class ScaryClassVanillaException
    {
        public static bool Prefix(ref System.Exception exception)
        {
            return LogMutePlugin.RegexExclude(exception.ToString());
        }
        public static IEnumerable<MethodBase> TargetMethods()
        {
            return [
                typeof(Debug).GetMethod(nameof(Debug.LogException), [typeof(System.Exception)]),
                typeof(Debug).GetMethod(nameof(Debug.LogException), [typeof(System.Exception), typeof(Object)]),
            ];
        }
    }

    [HarmonyPatch]
    public class ScaryClassModded
    {
        public static bool Prefix(ref object data)
        {
            if (data == null) data = "Null";
            return LogMutePlugin.RegexExclude(data.ToString());
        }
        public static MethodBase TargetMethod()
        {
            return typeof(ManualLogSource).GetMethod(nameof(ManualLogSource.Log), [typeof(LogLevel), typeof(object)]);
        }
    }
}
