using System;
using System.Runtime.CompilerServices;
using BetterErProspecting.Tracking;
using HarmonyLib;
using Vintagestory.GameContent;

namespace BetterErProspecting.Patches
{
	// Token: 0x02000008 RID: 8
	[HarmonyPatch(typeof(PropickReading), "ToHumanReadable")]
	[HarmonyPatchCategory("NewDensity")]
	public class ReadingToHumanReadableOreAdjustFactor
	{
		// Token: 0x0600003D RID: 61 RVA: 0x00003A42 File Offset: 0x00001C42
		[NullableContext(1)]
		private static void Prefix(PropickReading __instance)
		{
			if (((__instance != null) ? __instance.OreReadings : null) == null || BetterErProspect.Api == null)
			{
				return;
			}
			BetterErProspect.Api.ModLoader.GetModSystem<PptTracker>(true).AdjustFactor(__instance.OreReadings);
		}
	}
}
