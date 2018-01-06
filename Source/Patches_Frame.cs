using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Harmony;
using RimWorld;
using Verse;
using RimWorld.Planet;

namespace RTBricksDontVanish
{
	[HarmonyPatch(typeof(Frame))]
	[HarmonyPatch(nameof(Frame.FailConstruction))]
	static class Patch_FailConstruction
	{
		static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			var markerMethod = AccessTools.Method(typeof(Messages), nameof(Messages.Message), new Type[] { typeof(string), typeof(GlobalTargetInfo), typeof(MessageTypeDef) });
			var sneakyMethod = AccessTools.Method(typeof(Patch_FailConstruction), nameof(Patch_FailConstruction.ConditionalMessage));
			return Transpilers.MethodReplacer(instructions, markerMethod, sneakyMethod);
		}

		static void ConditionalMessage(string text, GlobalTargetInfo lookTarget, MessageTypeDef type)
		{
			if (ModSettings.notifyOnFailure)
			{
				Messages.Message(text, lookTarget, type);
			}
		}
	}
}
