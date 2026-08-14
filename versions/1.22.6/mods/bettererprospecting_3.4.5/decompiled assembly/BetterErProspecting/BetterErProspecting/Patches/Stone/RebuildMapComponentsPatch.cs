using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.GameContent;

namespace BetterErProspecting.Patches.Stone
{
	// Token: 0x0200000D RID: 13
	[NullableContext(1)]
	[Nullable(0)]
	[HarmonyPatch(typeof(OreMapLayer), "RebuildMapComponents")]
	[HarmonyPatchCategory("Always")]
	public class RebuildMapComponentsPatch
	{
		// Token: 0x06000049 RID: 73 RVA: 0x00004710 File Offset: 0x00002910
		private static double GetTotalFactor(PropickReading reading, string code)
		{
			return (double)RebuildMapComponentsPatch.GetTotalFactorMethod.Invoke(reading, new object[]
			{
				code
			});
		}

		// Token: 0x0600004A RID: 74 RVA: 0x0000472C File Offset: 0x0000292C
		private static bool Prefix(OreMapLayer __instance)
		{
			if (!((IWorldMapManager)AccessTools.Field(typeof(OreMapLayer), "mapSink").GetValue(__instance)).IsOpened)
			{
				return false;
			}
			ICoreClientAPI api = (ICoreClientAPI)AccessTools.Field(typeof(MapLayer), "api").GetValue(__instance);
			string filterByOreCode = (string)AccessTools.Field(typeof(OreMapLayer), "filterByOreCode").GetValue(__instance);
			List<MapComponent> wayPointComponents = AccessTools.Field(typeof(OreMapLayer), "wayPointComponents").GetValue(__instance) as List<MapComponent>;
			List<MapComponent> tmpWayPointComponents = AccessTools.Field(typeof(OreMapLayer), "tmpWayPointComponents").GetValue(__instance) as List<MapComponent>;
			foreach (MapComponent comp in tmpWayPointComponents)
			{
				wayPointComponents.Remove(comp);
			}
			foreach (MapComponent mapComponent in wayPointComponents)
			{
				mapComponent.Dispose();
			}
			wayPointComponents.Clear();
			for (int index = 0; index < __instance.ownPropickReadings.Count; index++)
			{
				PropickReading reading = __instance.ownPropickReadings[index];
				if (RebuildMapComponentsPatch.ShouldShowReading(reading, filterByOreCode))
				{
					wayPointComponents.Add(new OreMapComponent(index, reading, __instance, api, filterByOreCode));
				}
			}
			wayPointComponents.AddRange(tmpWayPointComponents);
			return false;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x000048B4 File Offset: 0x00002AB4
		private static bool ShouldShowReading(PropickReading reading, string filterByOreCode)
		{
			if (!BetterErProspect.Config.StoneSearchCreatesReadings)
			{
				if (Enumerable.All<string>(reading.OreReadings.Keys, (string k) => k.StartsWith("rock-")))
				{
					return false;
				}
			}
			bool result;
			if (filterByOreCode != null)
			{
				if (!(filterByOreCode == "ore-"))
				{
					if (!(filterByOreCode == "rock-"))
					{
						result = (RebuildMapComponentsPatch.GetTotalFactor(reading, filterByOreCode) > PropickReading.MentionThreshold);
					}
					else
					{
						result = Enumerable.Any<string>(reading.OreReadings.Keys, (string k) => k.StartsWith("rock-") && RebuildMapComponentsPatch.GetTotalFactor(reading, k) > PropickReading.MentionThreshold);
					}
				}
				else
				{
					result = Enumerable.Any<string>(reading.OreReadings.Keys, (string k) => !k.StartsWith("rock-") && RebuildMapComponentsPatch.GetTotalFactor(reading, k) > PropickReading.MentionThreshold);
				}
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x0400001D RID: 29
		private static readonly MethodInfo GetTotalFactorMethod = AccessTools.Method(typeof(PropickReading), "GetTotalFactor", null, null);
	}
}
