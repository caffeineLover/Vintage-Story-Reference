using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using BetterErProspecting.Config;
using HarmonyLib;
using ProspectTogether;
using ProspectTogether.Shared;
using Vintagestory.API.Client;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace BetterErProspecting.Patches.Compat.ProspectTogether
{
	// Token: 0x0200000F RID: 15
	[HarmonyPatch(typeof(OreMapLayer), "OnDataFromServer")]
	[HarmonyPatchCategory("ProspectTogetherCompat")]
	public class OreMapLayerPatch
	{
		// Token: 0x06000050 RID: 80 RVA: 0x00004A10 File Offset: 0x00002C10
		[NullableContext(1)]
		[HarmonyPrefix]
		[MethodImpl(8)]
		private static void Prefix(OreMapLayer __instance, byte[] data)
		{
			ICoreClientAPI capi = (ICoreClientAPI)typeof(OreMapLayer).GetField("capi", 36).GetValue(__instance);
			Dictionary<string, string> pageCodes = capi.ModLoader.GetModSystem<ModSystemOreMap>(true).prospectingMetaData.PageCodes;
			if (pageCodes == null)
			{
				capi.World.Logger.Error("pageCodes is null");
				return;
			}
			ProspectTogetherModSystem mod = capi.ModLoader.GetModSystem<ProspectTogetherModSystem>(true);
			if (mod == null)
			{
				return;
			}
			List<PropickReading> list = SerializerUtil.Deserialize<List<PropickReading>>(data);
			Dictionary<ChunkCoordinate, ProspectInfo> infos = new Dictionary<ChunkCoordinate, ProspectInfo>();
			foreach (PropickReading result in list)
			{
				List<OreOccurence> occurences = new List<OreOccurence>();
				foreach (KeyValuePair<string, OreReading> reading in result.OreReadings)
				{
					string pageCode = reading.Key;
					if (pageCodes.ContainsKey(reading.Key))
					{
						pageCode = DictExtensions.Get<string, string>(pageCodes, reading.Key, null);
					}
					if (reading.Value.TotalFactor > 0.025)
					{
						occurences.Add(new OreOccurence("game:ore-" + reading.Key, pageCode, (int)GameMath.Clamp(reading.Value.TotalFactor * Constants.LinearFactorValue, 0.0, 5.0) + 2, reading.Value.PartsPerThousand));
					}
					else if (reading.Value.TotalFactor > PropickReading.MentionThreshold)
					{
						occurences.Add(new OreOccurence("game:ore-" + reading.Key, pageCode, 1, reading.Value.PartsPerThousand));
					}
				}
				Vec3d pos = result.Position;
				int chunksize = 32;
				ChunkCoordinate coords;
				coords..ctor(pos.XInt / chunksize, pos.ZInt / chunksize);
				ProspectInfo info = new ProspectInfo(coords, occurences);
				infos[coords] = info;
			}
			mod.ClientStorage.PlayerProspected(Enumerable.ToList<ProspectInfo>(infos.Values));
		}
	}
}
