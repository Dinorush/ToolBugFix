using BepInEx.Unity.IL2CPP;
using System;

namespace ToolBugFix.Dependencies
{
    internal static class BSFDep
    {
        public const string PLUGIN_GUID = "randomuserhi.BioScannerFix";
        public static readonly bool Installed;

        internal static readonly Type[] PatchTypes = new Type[] { typeof(Patches.Bio.ScanPatches) };

        static BSFDep()
        {
            Installed = IL2CPPChainloader.Instance.Plugins.ContainsKey(PLUGIN_GUID);
        }
    }
}
