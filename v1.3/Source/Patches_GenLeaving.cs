using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
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
			__result = Math.Min(count, GenMath.RoundRandom(count * ModSettings.DeconstructionMaterialReturn));
		}
	}

	[HarmonyPatch]
	internal static class Patch_DoLeavingsForTerrain
	{
		private static MethodBase TargetMethod()
		{
			return typeof(GenLeaving).GetMethod(nameof(GenLeaving.DoLeavingsFor), new Type[] { typeof(TerrainDef), typeof(IntVec3), typeof(Map) });
		}

		private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> _instructions)
		{
			int patchState = 0;
			int index = 0;
			var instructions = _instructions.ToList();
			foreach (var instruction in instructions)
			{
				index++;
				if (patchState == 0 && instructions[index].opcode == OpCodes.Ldfld && (FieldInfo)instructions[index].operand == typeof(BuildableDef).GetField(nameof(BuildableDef.resourcesFractionWhenDeconstructed)))
				{
					patchState++;
					yield return new CodeInstruction(OpCodes.Call, typeof(ModSettings).GetProperty(nameof(ModSettings.DeconstructionMaterialReturn)).GetGetMethod());
					continue;
				}
				else if (patchState == 1)
				{
					patchState++;
					continue;
				}
				yield return instruction;
			}
		}
	}
}