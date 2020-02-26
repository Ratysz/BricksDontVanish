using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Reflection;
using Verse;

namespace RTBricksDontVanish
{
	[HarmonyPatch]
	internal static class Patch_FailConstruction
	{
		private static MethodBase TargetMethod()
		{
			return typeof(Frame).GetMethod(nameof(Frame.FailConstruction));
		}

		private static void Postfix()
		{
			ModSettings.volatile_ForceAltMessage = false;
		}

		private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			var markerMethod = AccessTools.Method(typeof(Messages), nameof(Messages.Message), new Type[] { typeof(string), typeof(LookTargets), typeof(MessageTypeDef), typeof(bool) });
			var sneakyMethod = AccessTools.Method(typeof(Patch_FailConstruction), nameof(Patch_FailConstruction.ConditionalMessage));
			return Transpilers.MethodReplacer(instructions, markerMethod, sneakyMethod);
		}

		private static void ConditionalMessage(string text, LookTargets lookTargets, MessageTypeDef type, bool historical = true)
		{
			if (ModSettings.notifyOnFailure)
			{
				if (ModSettings.volatile_ForceAltMessage || ModSettings.FailureMaterialReturn == 1f)
				{
					Messages.Message(text.Split(new[] { '.' })[0] + ".", lookTargets, type, historical);
				}
				else
				{
					Messages.Message(text, lookTargets, type, historical);
				}
			}
		}
	}

	/*[HarmonyPatch] // For testing purposes.
	static class Patch_FailConstruction2
	{
		static MethodBase TargetMethod()
		{
			return typeof(Frame).GetMethod(nameof(Frame.FailConstruction));
		}

		static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			var markerMethod = AccessTools.Property(typeof(Frame), nameof(Frame.WorkToBuild)).GetGetMethod();
			var sneakyMethod = AccessTools.Method(typeof(Patch_FailConstruction2), nameof(Patch_FailConstruction2.PatchedWorkToBuild));
			return Transpilers.MethodReplacer(instructions, markerMethod, sneakyMethod);
		}

		static float PatchedWorkToBuild(Frame instance)
		{
			return 1500f;
		}
	}*/
}