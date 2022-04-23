using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace RepairInTheZone
{
    public static class StatePathFinder
    {
        public static Pawn Pawn;
    }

    [HarmonyPatch(typeof(PathFinder), nameof(PathFinder.FindPath), new Type[] { 
        typeof(IntVec3), typeof(LocalTargetInfo), typeof(TraverseParms), typeof(PathEndMode), typeof(PathFinderCostTuning) })]
    public static class PathFinder_FindPath_Patch
    {
        public static void Prefix(TraverseParms traverseParms)
        {
            StatePathFinder.Pawn = traverseParms.pawn;
        }

        public static void Postfix()
        {
            StatePathFinder.Pawn = null;
        }
    }

    [HarmonyPatch]
    public static class PathFinder_CalculateAndAddDisallowedCorners_Patch
    {
        public static MethodBase TargetMethod()
        {
            return typeof(PathFinder).GetMethod("CalculateAndAddDisallowedCorners", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public static void Postfix(PathEndMode peMode, CellRect destinationRect, List<int> ___disallowedCornerIndices, Map ___map)
        {
            Pawn pawn = StatePathFinder.Pawn;
            Map map = ___map;
            List<int> list = ___disallowedCornerIndices;
            if (pawn != null && peMode != PathEndMode.Touch)
            {
                foreach (var cell in destinationRect)
                {
                    int index = map.cellIndices.CellToIndex(cell);
                    if (!cell.InAllowedArea(pawn) && !list.Contains(index))
                    {
                        list.Add(index);
                    }
                }
            }
        }
    }
}
