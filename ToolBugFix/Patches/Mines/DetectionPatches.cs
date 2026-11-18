using HarmonyLib;
using ToolBugFix.Utils;
using UnityEngine;

namespace ToolBugFix.Patches.Mines
{
    [HarmonyPatch]
    internal static class DetectionPatches
    {
        [HarmonyPatch(typeof(MineDeployerInstance), nameof(MineDeployerInstance.OnSpawnData))]
        [HarmonyPostfix]
        private static void Post_Spawn(MineDeployerInstance __instance)
        {
            __instance.m_courseNode = DoorUtil.GetCorrectNode(__instance.transform.position, __instance.m_courseNode);
        }

        // Copy/pasted vanilla logic, with an additional check before OnTargetDetected
        [HarmonyPatch(typeof(MineDeployerInstance_Detect_Laser), nameof(MineDeployerInstance_Detect_Laser.UpdateDetection))]
        [HarmonyPrefix]
        private static bool Pre_Detection(MineDeployerInstance_Detect_Laser __instance)
        {
            var core = __instance.m_core;
            var maxLineDistance = __instance.m_maxLineDistance;
            if (core.Mode == eStickyMineMode.Disabled || maxLineDistance == 0) return true;

            float scanDist;
            var lineRendererAlign = __instance.m_lineRendererAlign;
            if (Physics.SphereCast(lineRendererAlign.position, 0.1f, lineRendererAlign.forward, out var hitInfo, maxLineDistance, __instance.m_scanMask))
            {
                scanDist = hitInfo.distance;
                var collider = hitInfo.collider;
                if (__instance.m_enemyMask.IsInLayerMask(collider.gameObject))
                {
                    var damageable = collider.GetComponent<IDamageable>();
                    var agent = damageable.GetBaseAgent();
                    if (!DoorUtil.CanTouchNode(agent.Position, agent.CourseNode, core.CourseNode))
                        return false;

                    __instance.OnTargetDetected?.Invoke();
                    return false;
                }
            }
            else
            {
                scanDist = maxLineDistance;
            }

            if (scanDist != __instance.DetectionRange)
            {
                __instance.UpdateDetectionRange(scanDist);
            }
            return false;
        }
    }
}
