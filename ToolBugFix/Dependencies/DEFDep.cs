using BepInEx.Unity.IL2CPP;
using System;

namespace ToolBugFix.Dependencies
{
    internal static class DEFDep
    {
        public const string PLUGIN_GUID = "Localia.DoorEnemyFix";
        public static readonly bool Installed;

        internal static readonly Type[] PatchTypes = new Type[] { typeof(Patches.Foam.CollisionPatches), typeof(Patches.Foam.MergePatches), typeof(Patches.Mines.DetectionPatches) };

        static DEFDep()
        {
            Installed = IL2CPPChainloader.Instance.Plugins.ContainsKey(PLUGIN_GUID);
        }
    }
}
