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

		/*static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			bool patched = false;
			MethodInfo sneakyMethod = AccessTools.Property(typeof(ModSettings), nameof(ModSettings.FailureMaterialReturn)).GetGetMethod();
			foreach (var instr in instructions.ToList())
			{
				if (!patched && instr.opcode == OpCodes.Ldc_R4)
				{
					patched = true;
					yield return new CodeInstruction(OpCodes.Call, sneakyMethod);
				}
				else
				{
					yield return instr;
				}
			}
		}*/
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

		/*static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			int patchState = 0;
			MethodInfo sneakyMethod = AccessTools.Property(typeof(ModSettings), nameof(ModSettings.DeconstructionMaterialReturn)).GetGetMethod();
			foreach (var instr in instructions.ToList())
			{
				switch (patchState)
				{	// I'm almost sure inverting the state machine to have default as case 0 is more performant, but that's squeezing stones.
					case 0:
						if (instr.opcode == OpCodes.Ldarg_0)
						{
							patchState++;
							yield return new CodeInstruction(OpCodes.Call, sneakyMethod);
						}
						else
						{
							yield return instr;
						}
						break;
					case 1:
						patchState++;
						break;
					case 2:
						patchState++;
						break;
					case 3:
						patchState++;
						break;
					default:
						yield return instr;
						break;
				}
			}
		}*/
	}
}
