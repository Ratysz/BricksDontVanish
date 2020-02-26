using Harmony;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Verse;

namespace RTBricksDontVanish
{
	[HarmonyPatch]
	internal static class Patch_MakeNewToils
	{
		private static MethodBase TargetMethod()
		{
			return typeof(JobDriver_ConstructFinishFrame)
				.GetNestedTypes(AccessTools.all).FirstOrDefault()
				.GetNestedTypes(AccessTools.all).FirstOrDefault()
				.GetMethods(AccessTools.all).FirstOrDefault(method => method.Name.Contains("m__1"));
		}

		private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			var markerMethod = AccessTools.Property(typeof(Rand), nameof(Rand.Value)).GetGetMethod();
			var sneakyMethod = AccessTools.Method(typeof(Patch_MakeNewToils), nameof(Patch_MakeNewToils.PatchedRandValue));
			return Transpilers.MethodReplacer(instructions, markerMethod, sneakyMethod);
		}

		private static float PatchedRandValue()
		{
			if (ModSettings.FailureChanceScaling != 0f)
			{
				return Rand.Value / ModSettings.FailureChanceScaling;
			}
			return 100f;
		}
	}
}