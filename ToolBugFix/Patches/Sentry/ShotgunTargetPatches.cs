using HarmonyLib;
using UnityEngine;

namespace ToolBugFix.Patches.Sentry
{
    [HarmonyPatch]
    internal static class ShotgunTargetPatches
    {
        private static bool _inShotgunFire = false;
        private static Vector3 _cachedDir;
        [HarmonyPatch(typeof(SentryGunInstance_Firing_Bullets), nameof(SentryGunInstance_Firing_Bullets.TriggerSingleFireAudio))]
        [HarmonyWrapSafe]
        [HarmonyPostfix]
        private static void Pre_ShotgunFireBullet(SentryGunInstance_Firing_Bullets __instance)
        {
            if (__instance.m_fireMode != eWeaponFireMode.SentryGunShotgunSemi) return;
            if (!__instance.m_archetypeData.Sentry_FireTowardsTargetInsteadOfForward || !__instance.m_core.TryGetTargetAimPos(out var pos)) return;

            _inShotgunFire = true;
            _cachedDir = __instance.MuzzleAlign.forward;
            __instance.MuzzleAlign.forward = (pos - __instance.MuzzleAlign.position).normalized;
        }

        [HarmonyPatch(typeof(SentryGunInstance_Firing_Bullets), nameof(SentryGunInstance_Firing_Bullets.UpdateAmmo))]
        [HarmonyPostfix]
        private static void Post_ShotgunFireBullet(SentryGunInstance_Firing_Bullets __instance)
        {
            if (_inShotgunFire)
            {
                __instance.MuzzleAlign.forward = _cachedDir;
                _inShotgunFire = false;
            }
        }
    }
}
