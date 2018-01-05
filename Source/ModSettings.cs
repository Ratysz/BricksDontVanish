using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Verse;
using UnityEngine;

namespace RTBricksDontVanish
{
	public class ModSettings : Verse.ModSettings
	{
		// CONSTRUCTION:
		// Failure notification on/off
		// Fail chance % (of vanilla)
		// Non-destructive failure chance % (of fail chance)
		// Failure material loss % (of vanilla)

		// DECONSTRUCTION:
		// Material return % (of vanilla)

		public string SettingsCategory()
		{
			return "BricksDontVanish_SettingsCategory".Translate();
		}

		public void DoSettingsWindowContents(Rect rect)
		{

		}
	}
}
