using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace BetterErProspecting.Patches.Stone
{
	// Token: 0x0200000C RID: 12
	[HarmonyPatch(typeof(OreMapComponent), 3, new Type[]
	{
		typeof(int),
		typeof(PropickReading),
		typeof(OreMapLayer),
		typeof(ICoreClientAPI),
		typeof(string)
	})]
	[HarmonyPatchCategory("StoneReadings")]
	public class OreMapComponentCtorPatch
	{
		// Token: 0x06000047 RID: 71 RVA: 0x00004540 File Offset: 0x00002740
		[NullableContext(1)]
		private static bool Prefix(OreMapComponent __instance, int waypointIndex, PropickReading reading, OreMapLayer wpLayer, ICoreClientAPI capi, string filterByOreCode)
		{
			if (filterByOreCode != "ore-" && filterByOreCode != "rock-")
			{
				return true;
			}
			AccessTools.Field(typeof(OreMapComponent), "waypointIndex").SetValue(__instance, waypointIndex);
			AccessTools.Field(typeof(OreMapComponent), "reading").SetValue(__instance, reading);
			AccessTools.Field(typeof(OreMapComponent), "oreLayer").SetValue(__instance, wpLayer);
			AccessTools.Field(typeof(OreMapComponent), "viewPos").SetValue(__instance, new Vec2f());
			AccessTools.Field(typeof(OreMapComponent), "mvMat").SetValue(__instance, new Matrixf());
			AccessTools.Field(typeof(MapComponent), "capi").SetValue(__instance, capi);
			double highestFactor = 0.0;
			foreach (KeyValuePair<string, OreReading> kvp in reading.OreReadings)
			{
				if (((filterByOreCode == "rock-") ? kvp.Key.StartsWith("rock-") : (!kvp.Key.StartsWith("rock-"))) && kvp.Value.TotalFactor > highestFactor)
				{
					highestFactor = kvp.Value.TotalFactor;
				}
			}
			int num = GuiStyle.DamageColorGradient[(int)Math.Min(99.0, highestFactor * 150.0)];
			Vec4f colorVec = new Vec4f();
			ColorUtil.ToRGBAVec4f(num, ref colorVec);
			colorVec.W = 1f;
			AccessTools.Field(typeof(OreMapComponent), "color").SetValue(__instance, colorVec);
			return false;
		}
	}
}
