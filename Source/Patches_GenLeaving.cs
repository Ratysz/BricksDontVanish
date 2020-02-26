using Harmony;
using RimWorld;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Verse;

namespace RTBricksDontVanish
{
	[HarmonyPatch]
	internal static class Patch_GBRLC_FailConstruction
	{
		private static MethodBase TargetMethod()
		{
			return typeof(GenLeaving).GetMethods(AccessTools.all).FirstOrDefault(method => method.Name.Contains("m__6"));
		}

		private static void Postfix(ref int __result, int count)
		{
			if (Rand.Value < ModSettings.MildFailureChance)
			{
				__result = count;
				ModSettings.volatile_ForceAltMessage = true;
			}
			else
			{
				__result = Math.Min(count, GenMath.RoundRandom(count * ModSettings.FailureMaterialReturn));
			}
		}
	}

	[HarmonyPatch]
	internal static class Patch_GBRLC_Deconstruct
	{
		private static MethodBase TargetMethod()
		{
			return typeof(GenLeaving).GetNestedTypes(AccessTools.all)
				.FirstOrDefault(type => type.FullName.Contains("GetBuildingResourcesLeaveCalculator"))
				.GetMethods(AccessTools.all).FirstOrDefault(method => method.Name.Contains("m__0"));
		}

		private static void Postfix(ref int __result, int count)
		{
			if (ModSettings.deconstructionTrue100)
			{
				__result = Math.Min(count, GenMath.RoundRandom(count * ModSettings.DeconstructionMaterialReturn));
			}
			else
			{
				__result = GenMath.RoundRandom(Mathf.Min(count - 1, count * ModSettings.DeconstructionMaterialReturn));
			}
		}
	}
}