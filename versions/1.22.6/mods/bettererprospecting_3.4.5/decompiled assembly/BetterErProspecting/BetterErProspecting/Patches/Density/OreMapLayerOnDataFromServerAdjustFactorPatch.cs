using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BetterErProspecting.Tracking;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace BetterErProspecting.Patches.Density
{
	// Token: 0x0200000E RID: 14
	[HarmonyPatch(typeof(OreMapLayer), "OnDataFromServer")]
	[HarmonyPatchCategory("NewDensity")]
	public class OreMapLayerOnDataFromServerAdjustFactorPatch
	{
		// Token: 0x0600004E RID: 78 RVA: 0x000049B8 File Offset: 0x00002BB8
		[NullableContext(1)]
		[HarmonyPrefix]
		[HarmonyPriority(800)]
		private static void Prefix(ref byte[] data)
		{
			if (data != null)
			{
				ICoreClientAPI capi = BetterErProspect.Api as ICoreClientAPI;
				if (capi != null)
				{
					PptTracker pptTracker = capi.ModLoader.GetModSystem<PptTracker>(true);
					if (pptTracker == null)
					{
						return;
					}
					List<PropickReading> readings = SerializerUtil.Deserialize<List<PropickReading>>(data);
					if (readings == null)
					{
						return;
					}
					pptTracker.AdjustFactor(readings);
					data = SerializerUtil.Serialize<List<PropickReading>>(readings);
					return;
				}
			}
		}
	}
}
