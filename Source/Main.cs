using HarmonyLib;
using Verse;

namespace AllWindowsDraggable
{
    [StaticConstructorOnStartup]
    public static class Main
    {
        public const string ModIdentifier = "kathanon.RepairInTheZone";

        static Main()
        {
            var harmony = new Harmony(ModIdentifier);
            harmony.PatchAll();
        }
    }
}
