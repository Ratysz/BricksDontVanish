using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
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
			return typeof(GenLeaving).GetNestedTypes(AccessTools.all).First()
				.GetMethods(AccessTools.all).FirstOrDefault(method => method.Name.Contains("b__8_6"));
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
			return typeof(GenLeaving).GetNestedTypes(AccessTools.all).FirstOrDefault(nested => nested.Name.Contains("DisplayClass8_0"))
				.GetMethods(AccessTools.all).FirstOrDefault(method => method.Name.Contains("b__4"));
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