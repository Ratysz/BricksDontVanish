using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using System.Reflection.Emit;
using Harmony;
using RimWorld;
using Verse;

namespace RTBricksDontVanish
{
	[HarmonyPatch]
	static class Patch_GBRLC_Deconstruct
	{
		static MethodBase TargetMethod()
		{
			return typeof(GenLeaving).GetNestedTypes(AccessTools.all)
				.FirstOrDefault(type => type.FullName.Contains("GetBuildingResourcesLeaveCalculator"))
				.GetMethods(AccessTools.all).FirstOrDefault(method => method.Name.Contains("m__0"));
		}
	}

	[HarmonyPatch]
	static class Patch_GBRLC_FailConstruction
	{
		static MethodBase TargetMethod()
		{
			return typeof(GenLeaving)/*.GetNestedTypes(AccessTools.all)
				.FirstOrDefault(type => type.FullName.Contains("GetBuildingResourcesLeaveCalculator"))*/
				.GetMethods(AccessTools.all).FirstOrDefault(method => method.Name.Contains("m__5"));
		}
	}
}
