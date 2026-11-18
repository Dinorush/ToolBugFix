using Enemies;
using Gear;
using HarmonyLib;

namespace ToolBugFix.Patches.Bio
{
    internal static class ScanPatches
    {
        [HarmonyPatch(typeof(EnemyAgent), nameof(EnemyAgent.ScannerData), MethodType.Setter)]
        [HarmonyPostfix]
        private static void Postfix_Set_ScannerData(EnemyAgent __instance)
        {
            __instance.m_hasDirtyScannerColor = true;
            EnemyScannerDataObject scannerData = __instance.ScannerData;
            if (__instance.Locomotion.CurrentStateEnum != ES_StateEnum.Hibernate)
            {
                scannerData.m_soundIndex = 0;
            }
        }
    }
}
