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
		private static Frame volatile_instance;
		private static Pawn volatile_worker;

		static bool Prefix(Frame __instance, Pawn worker)
		{
			volatile_instance = __instance;
			volatile_worker = worker;
			return true;
		}

		static void Postfix()
		{
			ModSettings.volatile_ForceAltMessage = false;
			volatile_instance = null;
			volatile_worker = null;
		}

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
				if (ModSettings.volatile_ForceAltMessage || ModSettings.FailureMaterialReturn == 1f)
				{
					Messages.Message("MessageConstructionFailedNoWaste".Translate(new object[] {
						volatile_instance.Label, volatile_worker.LabelShort
					}), lookTarget, type);
				}
				else
				{
					Messages.Message(text, lookTarget, type);
				}
			}
		}
	}
}
