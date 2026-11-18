using HarmonyLib;

namespace ToolBugFix.Patches.Foam
{
    [HarmonyPatch]
    internal static class MergePatches
    {
        [HarmonyPatch(typeof(GlueGunProjectile), nameof(GlueGunProjectile.RemoveFromVolume))]
        [HarmonyPrefix]
        private static bool Pre_RemoveFromVolume(GlueGunProjectile __instance, float volRem, ref float __result)
        {
            if (!SNetwork.SNet.IsMaster) return true;

            var volumeDesc = __instance.m_volumeDesc;
            float totalVolume = volumeDesc.expandVolume + volumeDesc.volume;
            if (volRem < totalVolume) return true;

            __result = volRem - totalVolume;
            volumeDesc.expandVolume = 0;
            volumeDesc.volume = 0;
            __instance.m_volumeDesc = volumeDesc;
            ProjectileManager.WantToMergeGlueToNothing(__instance.SyncID);
            return false;
        }
    }
}
