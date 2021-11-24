using System.Reflection;
using RimWorld;
using Verse;
using HarmonyLib;

namespace TurretsHatePrisoners {
  [StaticConstructorOnStartup]
  public class TurretsHatePrisoners : Mod {
    public TurretsHatePrisoners(ModContentPack pack) : base(pack) {
    }
    static TurretsHatePrisoners() {
      Harmony harmony = new Harmony("name.bizna.rimworld.TurretsHatePrisoners");
      MethodInfo baseMethod, patchMethod;
      HarmonyMethod harmonyMethod;
      baseMethod = AccessTools.Method(typeof(Building_TurretGun), "IsValidTarget");
      patchMethod = typeof(TurretsHatePrisoners).GetMethod("IsValidTargetPatch");
      harmonyMethod = new HarmonyMethod(patchMethod);
      harmony.Patch(baseMethod, harmonyMethod, null);
    }
    private static bool IsHated(Pawn who) {
      return who.MentalStateDef == MentalStateDefOf.PanicFlee || PrisonBreakUtility.IsPrisonBreaking(who) || (ModsConfig.IdeologyActive && SlaveRebellionUtility.IsRebelling(who));
    }
    public static bool IsValidTargetPatch(ref bool __result, Thing t) {
      if(t is Pawn alice && IsHated(alice)) {
        __result = true;
        return false;
      }
      return true;
    }
  }
}
