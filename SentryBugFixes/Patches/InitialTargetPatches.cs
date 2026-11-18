using AK;
using HarmonyLib;

namespace SentryBugFixes.Patches
{
    [HarmonyPatch]
    internal static class InitialTargetPatches
    {
        [HarmonyPatch(typeof(SentryGunInstance), nameof(SentryGunInstance._OnGearSpawnComplete_b__65_0))]
        [HarmonyPrefix]
        private static bool InitialTargetFix(SentryGunInstance __instance)
        {
            __instance.Sound.Post(EVENTS.SENTRYGUN_DETECT);
            __instance.m_startFireTimer = Clock.Time + __instance.ArchetypeData.GetSentryStartFireDelay(__instance.Owner, __instance.m_detection.TargetIsTagged);
            __instance.m_visuals.SetVisualStatus(eSentryGunStatus.HasTarget);
            return false;
        }
    }
}
