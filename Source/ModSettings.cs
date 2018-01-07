using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using Verse;
using RimWorld;
using UnityEngine;

namespace RTBricksDontVanish
{
	public class ModSettings : Verse.ModSettings
	{
		// CONSTRUCTION:
		// + Failure notification on/off (on in vanilla)
		// + Fail chance % (multiplier of vanilla)
		// + Non-destructive failure chance % (of fail chance)
		// + Failure material return % (50% in vanilla)

		// DECONSTRUCTION:
		// + Deconstruction material return % (75% in vanilla)
		// + True 100% on/off (off in vanilla)

		public static bool notifyOnFailure = true;

		private static int failureChanceScalingPercentage = 100;
		public static float FailureChanceScaling
		{
			get
			{
				return failureChanceScalingPercentage / 100.0f;
			}
			set
			{
				failureChanceScalingPercentage = Mathf.RoundToInt(FailureChanceScaling * 100);
			}
		}

		private static int mildFailureChancePercentage = 0;
		public static float MildFailureChance
		{
			get
			{
				return mildFailureChancePercentage / 100.0f;
			}
			set
			{
				mildFailureChancePercentage = Mathf.RoundToInt(MildFailureChance * 100);
			}
		}

		private static int failureMaterialReturnPercentage = Mathf.RoundToInt((float)(
			typeof(GenLeaving).GetField("LeaveFraction_FailConstruction", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null)) * 100);
		public static float FailureMaterialReturn
		{
			get
			{
				return failureMaterialReturnPercentage / 100.0f;
			}
			set
			{
				failureMaterialReturnPercentage = Mathf.RoundToInt(FailureMaterialReturn * 100);
			}
		}

		private static int deconstructionMaterialReturnPercentage = Mathf.RoundToInt(GenLeaving.LeaveFraction_DeconstructDefault * 100);
		public static float DeconstructionMaterialReturn
		{
			get
			{
				return deconstructionMaterialReturnPercentage / 100.0f;
			}
			set
			{
				deconstructionMaterialReturnPercentage = Mathf.RoundToInt(DeconstructionMaterialReturn * 100);
			}
		}

		public static bool deconstructionTrue100 = false;

		public static bool volatile_ForceAltMessage = false;

		public override void ExposeData()
		{
			float failureChanceScaling = FailureChanceScaling;
			float mildFailureChance = MildFailureChance;
			float failureMaterialReturn = FailureMaterialReturn;
			float deconstructionMaterialReturn = DeconstructionMaterialReturn;
			base.ExposeData();
			Scribe_Values.Look(ref notifyOnFailure, "notifyOnFailure");
			Scribe_Values.Look(ref failureChanceScaling, "failureChanceScaling");
			Scribe_Values.Look(ref mildFailureChance, "mildFailureChance");
			Scribe_Values.Look(ref failureMaterialReturn, "failureMaterialReturn");
			Scribe_Values.Look(ref deconstructionMaterialReturn, "deconstructionMaterialReturn");
			Scribe_Values.Look(ref deconstructionTrue100, "deconstructionTrue100");
			Log.Message("[BricksDontVanish]: settings initialized, "
				+ "failure notifications are " + (notifyOnFailure ? "enabled" : "disabled") + ", "
				+ "failure chance scaling is " + failureChanceScaling + ", "
				+ "mild failure chance is " + mildFailureChance + ", "
				+ "failure material return is " + failureMaterialReturn + ", "
				+ "deconstruction material return is " + deconstructionMaterialReturn
				+ " (" + (deconstructionTrue100 ? "true" : "sub 1") + ")");
			failureChanceScalingPercentage = Mathf.RoundToInt(failureChanceScaling * 100);
			mildFailureChancePercentage = Mathf.RoundToInt(mildFailureChance * 100);
			failureMaterialReturnPercentage = Mathf.RoundToInt(failureMaterialReturn * 100);
			deconstructionMaterialReturnPercentage = Mathf.RoundToInt(deconstructionMaterialReturn * 100);
		}

		public string SettingsCategory()
		{
			return "BricksDontVanish_SettingsCategory".Translate();
		}

		public void DoSettingsWindowContents(Rect rect)
		{
			Listing_Standard list = new Listing_Standard(GameFont.Small);
			list.ColumnWidth = rect.width / 3;
			list.Begin(rect);
			list.Gap();
			list.CheckboxLabeled(
				"BricksDontVanish_NotifyOnFailureLabel".Translate(),
				ref notifyOnFailure,
				"BricksDontVanish_NotifyOnFailureTip".Translate());
			{
				string buffer = failureChanceScalingPercentage.ToString();
				Rect rectLine = list.GetRect(Text.LineHeight);
				Rect rectLeft = rectLine.LeftHalf().Rounded();
				Rect rectRight = rectLine.RightHalf().Rounded();
				Rect rectPercent = rectRight.RightPartPixels(Text.LineHeight);
				rectRight = rectRight.LeftPartPixels(rectRight.width - Text.LineHeight);
				Widgets.DrawHighlightIfMouseover(rectLine);
				TooltipHandler.TipRegion(rectLine, "BricksDontVanish_FailureChanceScalingTip".Translate());
				TextAnchor anchorBuffer = Text.Anchor;
				Text.Anchor = TextAnchor.MiddleLeft;
				Widgets.Label(rectLeft, "BricksDontVanish_FailureChanceScalingLabel".Translate());
				Text.Anchor = anchorBuffer;
				Widgets.TextFieldNumeric(rectRight, ref failureChanceScalingPercentage, ref buffer, 0, 100);
				Widgets.Label(rectPercent, "%");
			}
			{
				string buffer = mildFailureChancePercentage.ToString();
				Rect rectLine = list.GetRect(Text.LineHeight);
				Rect rectLeft = rectLine.LeftHalf().Rounded();
				Rect rectRight = rectLine.RightHalf().Rounded();
				Rect rectPercent = rectRight.RightPartPixels(Text.LineHeight);
				rectRight = rectRight.LeftPartPixels(rectRight.width - Text.LineHeight);
				Widgets.DrawHighlightIfMouseover(rectLine);
				TooltipHandler.TipRegion(rectLine, "BricksDontVanish_MildFailureChanceTip".Translate());
				TextAnchor anchorBuffer = Text.Anchor;
				Text.Anchor = TextAnchor.MiddleLeft;
				Widgets.Label(rectLeft, "BricksDontVanish_MildFailureChanceLabel".Translate());
				Text.Anchor = anchorBuffer;
				Widgets.TextFieldNumeric(rectRight, ref mildFailureChancePercentage, ref buffer, 0, 100);
				Widgets.Label(rectPercent, "%");
			}
			{
				string buffer = failureMaterialReturnPercentage.ToString();
				Rect rectLine = list.GetRect(Text.LineHeight);
				Rect rectLeft = rectLine.LeftHalf().Rounded();
				Rect rectRight = rectLine.RightHalf().Rounded();
				Rect rectPercent = rectRight.RightPartPixels(Text.LineHeight);
				rectRight = rectRight.LeftPartPixels(rectRight.width - Text.LineHeight);
				Widgets.DrawHighlightIfMouseover(rectLine);
				TooltipHandler.TipRegion(rectLine, "BricksDontVanish_FailureMaterialReturnTip".Translate());
				TextAnchor anchorBuffer = Text.Anchor;
				Text.Anchor = TextAnchor.MiddleLeft;
				Widgets.Label(rectLeft, "BricksDontVanish_FailureMaterialReturnLabel".Translate());
				Text.Anchor = anchorBuffer;
				Widgets.TextFieldNumeric(rectRight, ref failureMaterialReturnPercentage, ref buffer, 0, 100);
				Widgets.Label(rectPercent, "%");
			}
			{
				string buffer = deconstructionMaterialReturnPercentage.ToString();
				Rect rectLine = list.GetRect(Text.LineHeight);
				Rect rectLeft = rectLine.LeftHalf().Rounded();
				Rect rectRight = rectLine.RightHalf().Rounded();
				Rect rectPercent = rectRight.RightPartPixels(Text.LineHeight);
				rectRight = rectRight.LeftPartPixels(rectRight.width - Text.LineHeight);
				Widgets.DrawHighlightIfMouseover(rectLine);
				TooltipHandler.TipRegion(rectLine, "BricksDontVanish_DeconstructionMaterialReturnTip".Translate());
				TextAnchor anchorBuffer = Text.Anchor;
				Text.Anchor = TextAnchor.MiddleLeft;
				Widgets.Label(rectLeft, "BricksDontVanish_DeconstructionMaterialReturnLabel".Translate());
				Text.Anchor = anchorBuffer;
				Widgets.TextFieldNumeric(rectRight, ref deconstructionMaterialReturnPercentage, ref buffer, 0, 100);
				Widgets.Label(rectPercent, "%");
			}
			list.CheckboxLabeled(
				"BricksDontVanish_DeconstructionTrue100Label".Translate(),
				ref deconstructionTrue100,
				"BricksDontVanish_DeconstructionTrue100Tip".Translate());
			list.End();
		}
	}
}
