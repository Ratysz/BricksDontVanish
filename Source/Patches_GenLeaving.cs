using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using System.Reflection.Emit;
using Harmony;
using RimWorld;
using Verse;
using UnityEngine;

namespace RTBricksDontVanish
{
	[HarmonyPatch]
	static class Patch_GBRLC_FailConstruction
	{
		static MethodBase TargetMethod()
		{
			return typeof(GenLeaving).GetMethods(AccessTools.all).FirstOrDefault(method => method.Name.Contains("m__5"));
		}

		static void Postfix(ref int __result, int count)
		{
			__result = Math.Min(count, GenMath.RoundRandom(count * ModSettings.FailureMaterialReturn));
		}
	}

	[HarmonyPatch]
	static class Patch_GBRLC_Deconstruct
	{
		static MethodBase TargetMethod()
		{
			return typeof(GenLeaving).GetNestedTypes(AccessTools.all)
				.FirstOrDefault(type => type.FullName.Contains("GetBuildingResourcesLeaveCalculator"))
				.GetMethods(AccessTools.all).FirstOrDefault(method => method.Name.Contains("m__0"));
		}

		static void Postfix(ref int __result, int count)
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
