using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using Harmony;
using RimWorld;
using Verse;
using UnityEngine;

namespace RTBricksDontVanish
{
	public class Mod : Verse.Mod
	{
		public ModSettings settings;

		public Mod(ModContentPack content) : base(content)
		{
			Log.Message("---");
			Log.Message("type: " + typeof(Frame));
			foreach (var method in typeof(Frame).GetMethods(AccessTools.all))
			{
				Log.Message(" method: " + method);
			}
			Log.Message("---");

			var harmony = HarmonyInstance.Create("io.github.ratysz.bricksdontvanish");
			harmony.PatchAll(Assembly.GetExecutingAssembly());

			settings = GetSettings<ModSettings>();
		}

		public override string SettingsCategory()
		{
			return settings.SettingsCategory();
		}

		public override void DoSettingsWindowContents(Rect inRect)
		{
			settings.DoSettingsWindowContents(inRect);
		}
	}
}