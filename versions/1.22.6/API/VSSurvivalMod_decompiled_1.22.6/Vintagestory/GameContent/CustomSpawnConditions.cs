using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class CustomSpawnConditions : ModSystem
{
	private ICoreServerAPI sapi;

	public override bool ShouldLoad(EnumAppSide forSide)
	{
		return forSide == EnumAppSide.Server;
	}

	public override void StartServerSide(ICoreServerAPI api)
	{
		base.StartServerSide(api);
		sapi = api;
		sapi.Event.OnTrySpawnGroupNearOffthread += Event_OnTrySpawnGroupNearOffthread;
	}

	private bool Event_OnTrySpawnGroupNearOffthread(IBlockAccessor blockaccessor, EntityProperties properties, BlockPos pos)
	{
		JsonObject attributes = properties.Attributes;
		if (attributes != null && attributes["harshWinterSpawnRate"].Exists && sapi.World.Config.GetString("harshWinters").ToBool(defaultValue: true) && sapi.World.Calendar.GetSeason(pos) == EnumSeason.Winter)
		{
			float spawnRate = properties.Attributes?["harshWinterSpawnRate"].AsFloat() ?? 1f;
			return sapi.World.Rand.NextDouble() < (double)spawnRate;
		}
		float newWorldSpawnDelayHours = properties.Attributes?["newWorldSpawnDelayHours"].AsFloat() ?? 0f;
		if (sapi.World.Calendar.ElapsedHours < (double)newWorldSpawnDelayHours)
		{
			return false;
		}
		return true;
	}
}
