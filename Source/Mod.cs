using Harmony;
using System.Reflection;
using UnityEngine;
using Verse;

namespace RTBricksDontVanish
{
	public class Mod : Verse.Mod
	{
		public ModSettings settings;

		public Mod(ModContentPack content) : base(content)
		{
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