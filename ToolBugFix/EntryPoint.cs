using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using ToolBugFix.Dependencies;

namespace ToolBugFix
{
    [BepInPlugin("Dinorush." + MODNAME, MODNAME, "1.0.0")]
    [BepInDependency("dev.gtfomodding.gtfo-api", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency(BSFDep.PLUGIN_GUID, BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency(DEFDep.PLUGIN_GUID, BepInDependency.DependencyFlags.SoftDependency)]
    internal sealed class EntryPoint : BasePlugin
    {
        public const string MODNAME = "ToolBugFix";

        public override void Load()
        {
            var harmony = new Harmony(MODNAME);
            harmony.PatchAll();

            if (!BSFDep.Installed)
                foreach (var type in BSFDep.PatchTypes)
                    harmony.PatchAll(type);

            if (!DEFDep.Installed)
                foreach (var type in DEFDep.PatchTypes)
                    harmony.PatchAll(type);

            Log.LogMessage("Loaded " + MODNAME);
        }
    }
}